using Blazorise;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity;

public partial class ClaimTypeManagement
{
    protected List<TableColumn> ClaimTypeManagementTableColumns => TableColumns.Get<ClaimTypeManagement>();
    protected PageToolbar Toolbar { get; } = new();

    public ClaimTypeManagement()
    {
        ObjectMapperContext = typeof(AbpIdentityProBlazorModule);
        LocalizationResource = typeof(IdentityResource);

        CreatePolicyName = IdentityPermissions.ClaimTypes.Create;
        UpdatePolicyName = IdentityPermissions.ClaimTypes.Update;
        DeletePolicyName = IdentityPermissions.ClaimTypes.Delete;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Menu:IdentityManagement"]));
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["ClaimTypes"]));
        return ValueTask.CompletedTask;
    }

    protected override string GetDeleteConfirmationMessage(ClaimTypeDto entity)
    {
        return string.Format(L["ClaimTypeDeletionConfirmationMessage"], entity.Name);
    }

    protected override async ValueTask SetTableColumnsAsync()
    {
        ClaimTypeManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<ClaimTypeManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["Name"],
                        Data = nameof(ClaimTypeDto.Name),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["ValueType"],
                        Data = nameof(ClaimTypeDto.ValueType),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["Description"],
                        Data = nameof(ClaimTypeDto.Description),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["Regex"],
                        Data = nameof(ClaimTypeDto.Regex),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["Required"],
                        Data = nameof(ClaimTypeDto.Required),
                        Sortable = true,
                        Component = typeof(ClaimTypeRequiredBadgeComponent)
                    },
                    new TableColumn
                    {
                        Title = L["IsStatic"],
                        Data = nameof(ClaimTypeDto.IsStatic),
                        Sortable = true,
                        Component = typeof(ClaimTypeStaticBadgeComponent)
                    },
            });

        ClaimTypeManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.ClaimType));

        await base.SetTableColumnsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<ClaimTypeManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Clicked = (data) => OpenEditModalAsync(data.As<ClaimTypeDto>()),
                        Visible = (data) =>
                        {
                            var claimType = data.As<ClaimTypeDto>();
                            return !claimType.IsStatic && HasUpdatePermission;
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data)=> HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<ClaimTypeDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<ClaimTypeDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewClaimType"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }
}
