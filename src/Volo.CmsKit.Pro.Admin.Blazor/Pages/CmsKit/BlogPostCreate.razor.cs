using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Markdown;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Content;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class BlogPostCreate
{
    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IBlogPostAdminAppService BlogPostAdminAppService { get; set; }

    [Inject]
    protected IMediaDescriptorAdminAppService MediaDescriptorAdminAppService { get; set; }

    [Inject]
    protected IBlogAdminAppService BlogAdminAppService { get; set; }

    [Inject]
    protected IEntityTagAdminAppService EntityTagAdminAppService { get; set; }

    protected CreateBlogPostDto BlogPost { get; set; } = new CreateBlogPostDto();

    protected IFileEntry CoverImage { get; set; }

    protected List<BlogDto> Blogs { get; set; } = new List<BlogDto>();

    protected string Tags;

    protected Validations ValidationsRef;

    protected bool HasPublishPermission;

    protected Guid? BlogId { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Blogs = (await BlogAdminAppService.GetListAsync(new BlogGetListInput())).Items.ToList();
        HasPublishPermission = await AuthorizationService.IsGrantedAsync(CmsKitAdminPermissions.BlogPosts.Publish);
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

    protected virtual Task SaveToDraftAsync()
    {
        return CreateBlogPostAsync(BlogPostStatus.Draft);
    }

    protected virtual async Task WaitingForReviewAsync()
    {
        if (await Message.Confirm(L["BlogPostSendToReviewConfirmationMessage"]))
        {
            await CreateBlogPostAsync(BlogPostStatus.WaitingForReview);
        }
    }

    protected virtual async Task PublishBlogPostAsync()
    {
        if (await Message.Confirm(L["BlogPostPublishConfirmationMessage", BlogPost.Title]))
        {
            await CreateBlogPostAsync(BlogPostStatus.Published);
        }
    }

    protected virtual async Task CreateBlogPostAsync(BlogPostStatus status)
    {
        try
        {
            
            
            if (!await ValidationsRef.ValidateAll())
            {
                if(BlogId == null || BlogId == Guid.Empty)
                {
                    await Message.Error(L["PleaseSelectABlog"]);
                }
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

            BlogPost.BlogId = BlogId!.Value;
            var result = status switch {
                BlogPostStatus.Draft => await BlogPostAdminAppService.CreateAsync(BlogPost),
                BlogPostStatus.WaitingForReview => await BlogPostAdminAppService.CreateAndSendToReviewAsync(BlogPost),
                BlogPostStatus.Published => await BlogPostAdminAppService.CreateAndPublishAsync(BlogPost),
                _ => null
            };

            if (!Tags.IsNullOrWhiteSpace())
            {
                await EntityTagAdminAppService.SetEntityTagsAsync(new EntityTagSetDto
                {
                    EntityId = result.Id.ToString(),
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
    
    private void ValidateBlogId(ValidatorEventArgs e)
    {
        var blogId = e.Value as Guid?;
        
        e.Status = blogId == null || blogId == Guid.Empty ? ValidationStatus.Error : ValidationStatus.Success;
    }
}
