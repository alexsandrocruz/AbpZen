using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class BlogManagement
{
    [Inject]
    protected IBlogFeatureAdminAppService BlogFeatureAdminAppService { get; set; }

    protected List<TableColumn> BlogsManagementTableColumns => TableColumns.Get<BlogManagement>();
    protected PageToolbar Toolbar { get; } = new();
    
    protected List<BlogFeatureDto> BlogFeatures { get; set; }

    protected bool HasFeaturePermission;
    
    protected Guid EditingBlogId { get; set; }

    public Modal FeatureModal;

    public BlogManagement()
    {
        ObjectMapperContext = typeof(CmsKitProAdminBlazorModule);
        LocalizationResource = typeof(CmsKitResource);

        CreatePolicyName = CmsKitAdminPermissions.Blogs.Create;
        UpdatePolicyName = CmsKitAdminPermissions.Blogs.Update;
        DeletePolicyName = CmsKitAdminPermissions.Blogs.Delete;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Blogs"]));
        return ValueTask.CompletedTask;
    }

    protected async override Task SetPermissionsAsync()
    {
        HasFeaturePermission = await AuthorizationService.IsGrantedAsync(CmsKitAdminPermissions.Blogs.Features);
        await base.SetPermissionsAsync();
    }

    protected override string GetDeleteConfirmationMessage(BlogDto entity)
    {
        return string.Format(L["BlogDeletionConfirmationMessage"], entity.Name);
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewBlog"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<BlogManagement>()
            .AddRange(new EntityAction[] {
                new EntityAction {
                    Text = L["Features"],
                    Visible = (data) => HasFeaturePermission,
                    Clicked = async (data) => { await OpenFeatureModalAsync(data.As<BlogDto>()); }
                },
                new EntityAction {
                    Text = L["Edit"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenEditModalAsync(data.As<BlogDto>()); }
                },
                new EntityAction {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteEntityAsync(data.As<BlogDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<BlogDto>())
                }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        BlogsManagementTableColumns
            .AddRange(new TableColumn[] {
                new TableColumn {Title = L["Details"], Actions = EntityActions.Get<BlogManagement>()},
                new TableColumn {Title = L["Name"], Data = nameof(BlogDto.Name), Sortable = true},
                new TableColumn {Title = L["Slug"], Data = nameof(BlogDto.Slug), Sortable = true},
            });

        return base.SetTableColumnsAsync();
    }
    
    protected virtual bool IsFeatureAvailable(BlogFeatureDto feature)
    {
        var isAvailable = GlobalFeatureManager.Instance.Modules.CmsKit().GetFeatures().Any(a => a.FeatureName == feature.FeatureName) ?
            GlobalFeatureManager.Instance.IsEnabled(feature.FeatureName) :
            true;
        
        if (!isAvailable)
        {
            feature.IsEnabled = false;
        }

        return isAvailable;
    }
    
    protected virtual async Task OpenFeatureModalAsync(BlogDto entity)
    {
        try
        {
            EditingBlogId = entity.Id;
            BlogFeatures = await BlogFeatureAdminAppService.GetListAsync(entity.Id);

            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (FeatureModal != null)
                {
                    await FeatureModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task UpdateBlogFeatureAsync()
    {
        try
        {
            foreach (var feature in BlogFeatures)
            {
                await BlogFeatureAdminAppService.SetAsync(EditingBlogId, new BlogFeatureInputDto
                {
                    FeatureName = feature.FeatureName,
                    IsEnabled = feature.IsEnabled
                });
            }

            await CloseFeatureModalAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual Task ClosingFeatureModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }
    
    protected virtual Task CloseFeatureModalAsync()
    {
        return InvokeAsync(FeatureModal.Hide);
    }
}