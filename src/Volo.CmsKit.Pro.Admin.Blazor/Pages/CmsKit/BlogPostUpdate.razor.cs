using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Markdown;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Content;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Tags;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class BlogPostUpdate
{
    [Parameter]
    public Guid Id { get; set; }

    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IBlogPostAdminAppService BlogPostAdminAppService { get; set; }

    [Inject]
    protected IMediaDescriptorAdminAppService MediaDescriptorAdminAppService { get; set; }

    [Inject]
    protected IEntityTagAdminAppService EntityTagAdminAppService { get; set; }

    [Inject]
    protected ITagAppService TagAppService { get; set; }

    protected UpdateBlogPostDto BlogPost { get; set; }

    protected IFileEntry CoverImage { get; set; }

    public string CoverImageUrl { get; set; }

    protected string Tags;

    protected Validations ValidationsRef;

    protected DynamicWidgetMarkdown MarkdownRef;

    protected async override Task OnInitializedAsync()
    {
        BlogPost = ObjectMapper.Map<BlogPostDto, UpdateBlogPostDto>(await BlogPostAdminAppService.GetAsync(Id));
        Tags = (await TagAppService.GetAllRelatedTagsAsync(BlogPostConsts.EntityType, Id.ToString())).Select( x => x.Name).JoinAsString(",");
        CoverImageUrl = await GetCoverImageUrlAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["BlogPosts"]));
        return ValueTask.CompletedTask;
    }

    protected virtual Task OnTitleTextChangedAsync(string value)
    {
        BlogPost.Title = value;
        return OnSlugTextChangedAsync(value);
    }

    protected virtual Task OnSlugTextChangedAsync(string value)
    {
        BlogPost.Slug = value.Replace(" ", "-");
        return Task.CompletedTask;
    }

    protected virtual Task OnCoverImageChangedAsync(FileChangedEventArgs e)
    {
        CoverImage = e.Files.FirstOrDefault();
        return Task.CompletedTask;
    }

    protected virtual Task<string> GetCoverImageUrlAsync()
    {
        return Task.FromResult("/api/cms-kit/media/" + BlogPost.CoverImageMediaId);
    }

    protected virtual Task RemoveCoverImageAsync()
    {
        BlogPost.CoverImageMediaId = null;
        return Task.CompletedTask;
    }

    protected virtual async Task UpdateBlogPostAsync()
    {
        try
        {
            if (!await ValidationsRef.ValidateAll())
            {
                return;
            }

            if (CoverImage != null)
            {
                using(var stream = new MemoryStream())
                {
                    var createMediaInput = new CreateMediaInputWithStream
                    {
                        Name = CoverImage.Name
                    };

                    await CoverImage.WriteToStreamAsync(stream);
                    stream.Position = 0;
                    createMediaInput.File = new RemoteStreamContent(stream, createMediaInput.Name);
                    BlogPost.CoverImageMediaId = (await MediaDescriptorAdminAppService.CreateAsync(BlogPostConsts.EntityType, createMediaInput)).Id;
                }
            }

            await BlogPostAdminAppService.UpdateAsync(Id, BlogPost);

            if (!Tags.IsNullOrWhiteSpace())
            {
                await EntityTagAdminAppService.SetEntityTagsAsync(new EntityTagSetDto
                {
                    EntityId = Id.ToString(),
                    EntityType = BlogPostConsts.EntityType,
                    Tags = Tags.Split(",").ToList()
                });
            }

            NavigationManager.NavigateTo("Cms/BlogPosts");
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }
}
