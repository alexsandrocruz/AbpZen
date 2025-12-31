using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class PageManagement
{
    protected List<TableColumn> PagesManagementTableColumns => TableColumns.Get<PageManagement>();
    protected PageToolbar Toolbar { get; } = new();
    
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    
    public PageManagement()
    {
        ObjectMapperContext = typeof(CmsKitProAdminBlazorModule);
        LocalizationResource = typeof(CmsKitResource);

        CreatePolicyName = CmsKitAdminPermissions.Pages.Create;
        UpdatePolicyName = CmsKitAdminPermissions.Pages.Update;
        DeletePolicyName = CmsKitAdminPermissions.Pages.Delete;
    }
    
    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Pages"]));
        return ValueTask.CompletedTask;
    }

    protected override string GetDeleteConfirmationMessage(PageDto entity)
    {
        return string.Format(L["PageDeletionConfirmationMessage"]);
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewPage"],
            OpenCreatePageAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected virtual Task OpenCreatePageAsync()
    {
        NavigationManager.NavigateTo("Cms/Pages/Create");
        return Task.CompletedTask;
    }
    
    protected virtual Task OpenEditPageAsync(PageDto page)
    {
        NavigationManager.NavigateTo($"Cms/Pages/Update/{page.Id}");
        return Task.CompletedTask;
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<PageManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditPageAsync(data.As<PageDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<PageDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<PageDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        PagesManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Details"],
                        Actions = EntityActions.Get<PageManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["Title"],
                        Data = nameof(PageDto.Title),
                        Sortable = true
                    },
                    new TableColumn
                    {
                        Title = L["Slug"],
                        Data = nameof(PageDto.Slug),
                        Sortable = true
                    },
                    new TableColumn
                    {
                        Title = L["CreationTime"],
                        Data = nameof(PageDto.CreationTime),
                        Sortable = true
                    },
                    new TableColumn
                    {
                        Title = L["LastModificationTime"],
                        Data = nameof(PageDto.LastModificationTime),
                        Sortable = true
                    },
            });

        return base.SetTableColumnsAsync();
    }
}