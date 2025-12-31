using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.AuditLogging.Blazor.Components;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.OpenIddict.Permissions;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict.Pro.Blazor.Pages;

public partial class ScopeManagement
{
    protected const string EntityTypeFullName = "Volo.Abp.OpenIddict.Scopes.OpenIddictScope";
    
    [Inject]
    protected IUiMessageService UiMessageService { get; set; }
    
    protected PageToolbar Toolbar { get; } = new();
    
    protected ScopeModalView CreateInput = new();

    protected ScopeModalView UpdateInput = new();
    
    protected bool HasViewChangeHistoryPermission { get; set; }
    
    protected List<TableColumn> ScopeManagementTableColumns => TableColumns.Get<ScopeManagement>();
    
    protected EntityChangeHistoryModal EntityChangeHistoryModal;
    
    public ScopeManagement()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
        ObjectMapperContext = typeof(AbpOpenIddictProBlazorModule);

        CreatePolicyName = AbpOpenIddictProPermissions.Scope.Create;
        UpdatePolicyName = AbpOpenIddictProPermissions.Scope.Update;
        DeletePolicyName = AbpOpenIddictProPermissions.Scope.Delete;
    }
    
    protected async override Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasViewChangeHistoryPermission = await AuthorizationService.IsGrantedAsync(AbpOpenIddictProPermissions.Scope.ViewChangeHistory);
    }

    protected override Task OpenCreateModalAsync()
    {
        CreateInput = new ScopeModalView();
        return base.OpenCreateModalAsync();
    }

    protected override Task OnCreatingEntityAsync()
    {
        NewEntity = ObjectMapper.Map<ScopeModalView, CreateScopeInput>(CreateInput);
        return base.OnCreatingEntityAsync();
    }

    protected override UpdateScopeInput MapToEditingEntity(ScopeDto entityDto)
    {
        UpdateInput = ObjectMapper.Map<ScopeDto, ScopeModalView>(entityDto);
        return base.MapToEditingEntity(entityDto);;
    }

    protected override Task OnUpdatingEntityAsync()
    {
        EditingEntity = ObjectMapper.Map<ScopeModalView, UpdateScopeInput>(UpdateInput);
        return base.OnUpdatingEntityAsync();
    }
    
    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Menu:OpenIddict"]));
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Scopes"]));

        return base.SetBreadcrumbItemsAsync();
    }

    protected override string GetDeleteConfirmationMessage(ScopeDto entity)
    {
        return L["ScopeDeletionWarningMessage", entity.Name];
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewScope"], OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<ScopeManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => await OpenEditModalAsync(data.As<ScopeDto>())
                    },
                    new EntityAction
                    {
                        Text = L["Permission:ViewChangeHistory"],
                        Visible = (data) => HasViewChangeHistoryPermission,
                        Clicked = async (data) => await EntityChangeHistoryModal.OpenAsync(EntityTypeFullName, data.As<ScopeDto>().Id.ToString())
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<ScopeDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<ScopeDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        ScopeManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<ScopeManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["Name"],
                        Data = nameof(ScopeDto.Name),
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName"],
                        Data = nameof(ScopeDto.DisplayName),
                    },
                    new TableColumn
                    {
                        Title = L["Description"],
                        Data = nameof(ScopeDto.Description),
                    }
            });

        return base.SetEntityActionsAsync();
    }
}

public class ScopeModalView : ExtensibleObject
{
    public Guid Id { get; set; }
    
    [Required]
    [RegularExpression(@"\w+", ErrorMessage = "TheScopeNameCannotContainSpaces")]
    public string Name { get; set; }

    public string DisplayName { get; set; }
    
    public string Description { get; set; }

    public string Resources { get; set; }
}