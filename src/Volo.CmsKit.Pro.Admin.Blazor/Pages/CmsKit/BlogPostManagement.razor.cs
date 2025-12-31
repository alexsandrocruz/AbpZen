using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class BlogPostManagement
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    
    protected List<TableColumn> BlogPostsManagementTableColumns => TableColumns.Get<BlogPostManagement>();
    protected PageToolbar Toolbar { get; } = new();
    
    protected BlogPostStatus[] BlogPostStatuses { get; set; } =
    {
        BlogPostStatus.Draft,
        BlogPostStatus.Published,
        BlogPostStatus.WaitingForReview
    };
    
    protected bool HasPublishPermission { get; set; }
    
    public BlogPostManagement()
    {
        ObjectMapperContext = typeof(CmsKitProAdminBlazorModule);
        LocalizationResource = typeof(CmsKitResource);

        CreatePolicyName = CmsKitAdminPermissions.BlogPosts.Create;
        UpdatePolicyName = CmsKitAdminPermissions.BlogPosts.Update;
        DeletePolicyName = CmsKitAdminPermissions.BlogPosts.Delete;
    }

    protected virtual async Task OnSelectedStatusChangedAsync(BlogPostStatus? status)
    {
        GetListInput.Status = status;
        await SearchEntitiesAsync();
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["BlogPosts"]));
        return ValueTask.CompletedTask;
    }

    protected async override Task SetPermissionsAsync()
    {
        HasPublishPermission = await AuthorizationService.IsGrantedAsync(CmsKitAdminPermissions.BlogPosts.Publish);
        await base.SetPermissionsAsync();
    }

    protected virtual Task OpenCreatePageAsync()
    {
        NavigationManager.NavigateTo("Cms/BlogPosts/Create");
        return Task.CompletedTask;
    }
    
    protected virtual Task OpenEditPageAsync(BlogPostListDto blogPost)
    {
        NavigationManager.NavigateTo($"Cms/BlogPosts/update/{blogPost.Id}");
        return Task.CompletedTask;
    }

    protected virtual async Task PublishAsync(BlogPostListDto blogPost)
    {
        if (await Message.Confirm(L["BlogPostPublishConfirmationMessage", blogPost.Title]))
        {
            await AppService.PublishAsync(blogPost.Id);
            await Message.Success(L["SuccessfullyPublished"]);
            await GetEntitiesAsync();
            await InvokeAsync(StateHasChanged);
        }
    }
    
    protected virtual async Task SendToReviewAsync(BlogPostListDto blogPost)
    {
        if (await Message.Confirm(L["BlogPostPublishConfirmationMessage", blogPost.Title]))
        {
            await AppService.SendToReviewAsync(blogPost.Id);
            await Message.Success(L["BlogPostSendToReviewSuccessMessage", blogPost.Title]);
            await GetEntitiesAsync();
            await InvokeAsync(StateHasChanged);
        }
    }
    
    protected virtual async Task DraftAsync(BlogPostListDto blogPost)
    {
        if (await Message.Confirm(L["BlogPostDraftConfirmationMessage", blogPost.Title]))
        {
            await AppService.DraftAsync(blogPost.Id);
            await Message.Success(L["SavedSuccessfully"]);
            await GetEntitiesAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    protected override string GetDeleteConfirmationMessage(BlogPostListDto entity)
    {
        return string.Format(L["BlogPostDeletionConfirmationMessage"], entity.Title);
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewBlogPost"],
            OpenCreatePageAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }
    
    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<BlogPostManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditPageAsync(data.As<BlogPostListDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Publish"],
                        Visible = (data) => HasPublishPermission && data.As<BlogPostListDto>().Status != BlogPostStatus.Published,
                        Clicked = async (data) => { await PublishAsync(data.As<BlogPostListDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["SendToReview"],
                        Visible = (data) => !HasPublishPermission && data.As<BlogPostListDto>().Status == BlogPostStatus.Draft,
                        Clicked = async (data) => { await SendToReviewAsync(data.As<BlogPostListDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Draft"],
                        Visible = (data) => HasUpdatePermission && data.As<BlogPostListDto>().Status != BlogPostStatus.Draft,
                        Clicked = async (data) => { await DraftAsync(data.As<BlogPostListDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<BlogPostListDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<BlogPostListDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        BlogPostsManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Details"],
                        Actions = EntityActions.Get<BlogPostManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["Blog"],
                        Data = nameof(BlogPostListDto.BlogName)
                    },
                    new TableColumn
                    {
                        Title = L["Title"],
                        Data = nameof(BlogPostListDto.Title),
                        Sortable = true
                    },
                    new TableColumn
                    {
                        Title = L["Slug"],
                        Data = nameof(BlogPostListDto.Slug),
                        Sortable = true
                    },
                    new TableColumn
                    {
                        Title = L["CreationTime"],
                        Data = nameof(BlogPostListDto.CreationTime),
                        Sortable = true
                    },
                    new TableColumn
                    {
                        Title = L["Status"],
                        Data = nameof(BlogPostListDto.Status),
                        Sortable = true
                    },
            });

        return base.SetTableColumnsAsync();
    }
}