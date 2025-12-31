using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.ObjectExtending;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Admin.Pages;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class PageUpdate
{
    [Parameter]
    public Guid Id { get; set; }

    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);

    [Inject]
    protected IPageAdminAppService PageAdminAppService { get; set; }

    [Inject]
    protected IMediaDescriptorAdminAppService MediaDescriptorAdminAppService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected UpdatePageInputDto EditingPage { get; set; }

    protected Validations ValidationsRef;

    protected DynamicWidgetMarkdown MarkdownRef;

    protected string[] HideIcons = new[] {"bold"};

    protected string SelectedTab = "content";

    protected async override Task OnInitializedAsync()
    {
        await GetEditingPageAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }  

    protected virtual async Task GetEditingPageAsync()
    {
        var page = await PageAdminAppService.GetAsync(Id);

        EditingPage = new UpdatePageInputDto
        {
            ConcurrencyStamp = page.ConcurrencyStamp,
            Content = page.Content,
            Script = page.Script,
            Style = page.Style,
            Title = page.Title,
            Slug = page.Slug
        };
        page.MapExtraPropertiesTo(EditingPage);
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Pages"]));
        return ValueTask.CompletedTask;
    }

    protected virtual async Task UpdatePageAsync()
    {
        try
        {
            if (await ValidationsRef.ValidateAll())
            {
                await PageAdminAppService.UpdateAsync(Id, EditingPage);
                NavigationManager.NavigateTo("cms/pages");
            }
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }
}
