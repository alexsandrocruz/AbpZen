using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Blazorise;
using Blazorise.Markdown;
using Bunit;
using Bunit.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.CmsKit.Admin.Contents;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Contents;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class DynamicWidgetMarkdown
{
    [Inject]
    protected IMediaDescriptorAdminAppService MediaDescriptorAdminAppService { get; set; }

    [Inject]
    protected IOptions<CmsKitContentWidgetOptions> Options { get; set; }

    [Inject]
    protected IContentRender ContentRender { get; set; }

    [Inject]
    protected IJSRuntime JsRuntime { get; set; }

    [Parameter]
    public string Value { get; set; }

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    protected Dictionary<int, Stream> UploadImages = new Dictionary<int, Stream>();

    protected Markdown MarkdownRef;

    protected List<MarkdownAction> MarkdownToolbarButtons { get; set; }

    protected Modal AddWidgetModalRef;

    protected ContentViewModel ViewModel { get; set; }

    protected Dictionary<string, string> Widgets { get; set; } = new();

    protected override void OnInitialized()
    {
        MarkdownToolbarButtons = new List<MarkdownAction>() {
            MarkdownAction.Bold,
            MarkdownAction.Italic,
            MarkdownAction.Heading,
            MarkdownAction.Code,
            MarkdownAction.Quote,
            MarkdownAction.OrderedList,
            MarkdownAction.UnorderedList,
            MarkdownAction.Link,
            MarkdownAction.UploadImage,
            MarkdownAction.Table,
            MarkdownAction.Preview,
            MarkdownAction.Guide
        };
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitializeMarkdownAsync(Value);
        }
    }

    protected async Task OnValueChanged(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);

        if (!Value.IsNullOrWhiteSpace() && Value.Contains("Placeholder.jpg"))
        {
            await MarkdownRef.SetValueAsync(Value.Replace("Placeholder.jpg", string.Empty));
        }
    }

    public virtual async Task InitializeMarkdownAsync(string content)
    {
        if (content.IsNullOrWhiteSpace())
        {
            return;
        }

        var initialized = false;
        while (!initialized)
        {
            if (MarkdownRef == null)
            {
                continue;
            }
            if (await MarkdownRef.GetValueAsync() == content)
            {
                initialized = true;
                continue;
            }

            if (await MarkdownRef.GetValueAsync() != string.Empty)
            {
                continue;
            }

            await MarkdownRef.SetValueAsync(content);
            initialized = true;
        }
    }

    protected virtual Task OnCustomButtonClicked( MarkdownButtonEventArgs eventArgs )
    {
        return eventArgs.Name switch {
            "W" => OpenAddWidgetModalAsync(),
            _ => Task.CompletedTask
        };
    }

    protected virtual Task OpenAddWidgetModalAsync()
    {
        var widgets = Options.Value.WidgetConfigs
            .Select(n =>
                new ContentWidgetDto
                {
                    Key = n.Key,
                    Details = new WidgetDetailDto() { EditorComponentName = n.Value.EditorComponentName, Name = n.Value.Name },

                }).ToList();

        ViewModel = new ContentViewModel()
        {
            Details = widgets.Select(p => p.Details).ToList()
        };

        Widgets = widgets.ToDictionary(p => p.Details.Name, p => p.Key);

        return AddWidgetModalRef.Show();
    }

    protected virtual async Task<string> PreviewMarkdownAsync(string plainText)
    {
        plainText = await ContentRender.RenderAsync(plainText);

        return plainText;
    }

    protected virtual Task CloseAddWidgetModalAsync()
    {
        return InvokeAsync(AddWidgetModalRef.Hide);
    }

    protected virtual async Task AddWidgetAsync()
    {
        if (ViewModel.Widget.IsNullOrWhiteSpace())
        {
            return;
        }

        var widget = ViewModel.Details.FirstOrDefault(x => x.Name == ViewModel.Widget);
        var widgetProperties = string.Empty;

        if (widget != null && !widget.EditorComponentName.IsNullOrWhiteSpace())
        {
            var widgetForm = await JsRuntime.InvokeAsync<IJSObjectReference>("$","#widgetForm");
            var nameValues = await widgetForm.InvokeAsync<NameValue[]>("serializeArray");
            foreach (var nameValue in nameValues)
            {
                widgetProperties += $"{nameValue.Name}=\"{nameValue.Value}\" ";
            }
        }

        var widgetText = "[Widget Type=\"" + Widgets[ViewModel.Widget] + "\" " + widgetProperties + "]";
        Value += widgetText;
        await InvokeAsync(async() =>
        {
            await OnValueChanged(Value);
        });
        await SetMarkdownValueAsync(Value);
        await CloseAddWidgetModalAsync();
    }

    protected virtual Task ClosingAddWidgetModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual async Task OnImageUploadChangedAsync(FileChangedEventArgs e)
    {
        try
        {
            if (e.Files.Length > 1)
            {
                return;
            }

            foreach (var file in e.Files)
            {
                UploadImages[file.Id] = new MemoryStream();
                await file.WriteToStreamAsync(UploadImages[file.Id]);

            }
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception);
        }

    }

    protected virtual async Task OnImageUploadEndedAsync(FileEndedEventArgs e)
    {
        using (var stream = UploadImages[e.File.Id])
        {
            stream.Position = 0;
            var result = await MediaDescriptorAdminAppService.CreateAsync(BlogPostConsts.EntityType, new CreateMediaInputWithStream
            {
                Name = e.File.Name,
                File = new RemoteStreamContent(stream, e.File.Name, e.File.Type)
            });

            e.File.UploadUrl = await GetUploadUrlAsync($"/api/cms-kit/media/{result.Id}Placeholder.jpg");

            UploadImages.Remove(e.File.Id);
        }
    }

    protected virtual Task<string> GetUploadUrlAsync(string url)
    {
        return Task.FromResult(url);
    }

    public async Task SetMarkdownValueAsync(string value)
    {
        await MarkdownRef.SetValueAsync(value);
    }

    public class ContentViewModel
    {
        public string Widget { get; set; }

        public List<WidgetDetailDto> Details { get; set; }
    }
}
