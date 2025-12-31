using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using OpenIddict.Abstractions;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.AuditLogging.Blazor.Components;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Applications.Dtos;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.OpenIddict.Permissions;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.OpenIddict.Pro.Blazor.Pages;

public partial class ApplicationManagement
{
    protected const string EntityTypeFullName = "Volo.Abp.OpenIddict.Applications.OpenIddictApplication";
    protected const string PermissionProviderName = "C";

    [Inject]
    protected IUiMessageService UiMessageService { get; set; }

    [Inject]
    protected IScopeAppService ScopeAppService { get; set; }

    protected PermissionManagementModal PermissionManagementModal;

    protected EntityChangeHistoryModal EntityChangeHistoryModal;

    protected bool HasManagePermissionsPermission { get; set; }

    protected PageToolbar Toolbar { get; } = new();

    protected ApplicationModalView CreateInput = new();

    protected ApplicationModalView UpdateInput = new();

    protected Dictionary<string, bool> Scopes = new ();

    protected bool HasViewChangeHistoryPermission { get; set; }

    protected List<TableColumn> ApplicationManagementTableColumns => TableColumns.Get<ApplicationManagement>();

    protected Dictionary<string, string> ApplicationTypes;
    protected Dictionary<string, string> ClientTypes;

    protected Dictionary<string, string> ConsentTypes;

    protected Modal TokenLifetimeModal;
    protected Guid TokenLifetimeEntityId = default!;
    protected ApplicationTokenLifetimeModalView TokenLifetime { get; set; } = new();
    protected Validations TokenLifetimeValidationsRef;

    public ApplicationManagement()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
        ObjectMapperContext = typeof(AbpOpenIddictProBlazorModule);

        CreatePolicyName = AbpOpenIddictProPermissions.Application.Create;
        UpdatePolicyName = AbpOpenIddictProPermissions.Application.Update;
        DeletePolicyName = AbpOpenIddictProPermissions.Application.Delete;

        ApplicationTypes = new Dictionary<string, string>
        {
            { "Web", OpenIddictConstants.ApplicationTypes.Web },
            { "Native", OpenIddictConstants.ApplicationTypes.Native }
        };
        ClientTypes = new Dictionary<string, string>
        {
            { "Confidential client", OpenIddictConstants.ClientTypes.Confidential },
            { "Public client", OpenIddictConstants.ClientTypes.Public }
        };
        ConsentTypes = new Dictionary<string, string>
        {
            { "Explicit consent", OpenIddictConstants.ConsentTypes.Explicit },
            { "External consent", OpenIddictConstants.ConsentTypes.External },
            { "Implicit consent", OpenIddictConstants.ConsentTypes.Implicit },
            { "Systematic consent", OpenIddictConstants.ConsentTypes.Systematic }
        };
    }

    protected async override Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasViewChangeHistoryPermission = await AuthorizationService.IsGrantedAsync(AbpOpenIddictProPermissions.Application.ViewChangeHistory);
        HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(AbpOpenIddictProPermissions.Application.ManagePermissions);
    }

    protected async override Task OpenCreateModalAsync()
    {
        Scopes = (await ScopeAppService.GetAllScopesAsync()).ToDictionary(x => x.Name, x => false);
        CreateInput = new ApplicationModalView();
        CreateInput.ApplicationType = ApplicationTypes.First().Value;
        await base.OpenCreateModalAsync();
    }

    protected override Task OnCreatingEntityAsync()
    {
        CreateInput.Scopes = Scopes.Where(x => x.Value).Select(x => x.Key).ToHashSet();
        NewEntity = ObjectMapper.Map<ApplicationModalView, CreateApplicationInput>(CreateInput);
        return base.OnCreatingEntityAsync();
    }

    protected async override Task OpenEditModalAsync(ApplicationDto entity)
    {
        await base.OpenEditModalAsync(entity);
        Scopes = (await ScopeAppService.GetAllScopesAsync()).ToDictionary(x => x.Name, x => EditingEntity.Scopes.Any(s => s == x.Name));
        await InvokeAsync(StateHasChanged);
    }

    protected virtual Task ClosingTokenLifetimeModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual Task CloseTokenLifetimeModalAsync()
    {
        InvokeAsync(TokenLifetimeModal!.Hide);
        return Task.CompletedTask;
    }

    protected async Task OpenTokenLifetimeModalAsync(ApplicationDto entity)
    {
        TokenLifetime = ObjectMapper.Map<ApplicationTokenLifetimeDto, ApplicationTokenLifetimeModalView>(await AppService.GetTokenLifetimeAsync(entity.Id));
        TokenLifetimeEntityId = entity.Id;
        await TokenLifetimeModal.Show();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task UpdateTokenLifetimeAsync()
    {
        try
        {
            var validate = true;
            if (TokenLifetimeValidationsRef != null)
            {
                validate = await TokenLifetimeValidationsRef.ValidateAll();
            }

            if (validate)
            {
                await CheckUpdatePolicyAsync();
                var dto = ObjectMapper.Map<ApplicationTokenLifetimeModalView, ApplicationTokenLifetimeDto>(TokenLifetime);
                await AppService.SetTokenLifetimeAsync(TokenLifetimeEntityId, dto);
                await GetEntitiesAsync();

                await InvokeAsync(TokenLifetimeModal!.Hide);
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected override UpdateApplicationInput MapToEditingEntity(ApplicationDto entityDto)
    {
        UpdateInput = ObjectMapper.Map<ApplicationDto, ApplicationModalView>(entityDto);
        if (UpdateInput.ApplicationType.IsNullOrWhiteSpace())
        {
            UpdateInput.ApplicationType = ApplicationTypes.First().Value;
        }
        return base.MapToEditingEntity(entityDto);;
    }

    protected override Task OnUpdatingEntityAsync()
    {
        UpdateInput.Scopes = Scopes.Where(x => x.Value).Select(x => x.Key).ToHashSet();
        EditingEntity = ObjectMapper.Map<ApplicationModalView, UpdateApplicationInput>(UpdateInput);
        return base.OnUpdatingEntityAsync();
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Menu:OpenIddict"]));
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Applications"]));

        return base.SetBreadcrumbItemsAsync();
    }

    protected override string GetDeleteConfirmationMessage(ApplicationDto entity)
    {
        return L["ApplicationDeletionWarningMessage", entity.ClientId];
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewApplication"], OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<ApplicationManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => await OpenEditModalAsync(data.As<ApplicationDto>())
                    },
                    new EntityAction
                    {
                        Text = L["TokenLifetime"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => await OpenTokenLifetimeModalAsync(data.As<ApplicationDto>())
                    },
                    new EntityAction
                    {
                        Text = L["Permissions"],
                        Visible = (data) => HasManagePermissionsPermission,
                        Clicked = async (data) => await PermissionManagementModal.OpenAsync(PermissionProviderName, data.As<ApplicationDto>().ClientId)
                    },
                    new EntityAction
                    {
                        Text = L["Permission:ViewChangeHistory"],
                        Visible = (data) => HasViewChangeHistoryPermission,
                        Clicked = async (data) => await EntityChangeHistoryModal.OpenAsync(EntityTypeFullName, data.As<ApplicationDto>().Id.ToString())
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<ApplicationDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<ApplicationDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        ApplicationManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<ApplicationManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["ApplicationType"],
                        Data = nameof(ApplicationDto.ApplicationType),
                    },
                    new TableColumn
                    {
                        Title = L["ClientId"],
                        Data = nameof(ApplicationDto.ClientId),
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName"],
                        Data = nameof(ApplicationDto.DisplayName),
                    },
                    new TableColumn
                    {
                        Title = L["ClientType"],
                        Data = nameof(ApplicationDto.ClientType),
                    }
            });

        return base.SetEntityActionsAsync();
    }
}

public class ApplicationModalView : ExtensibleObject
{
    [Required]
    public string ApplicationType { get; set; }

    [Required]
    public string ClientId { get; set; }

    [Required]
    public string DisplayName { get; set; }

    private string _clientType;

    public string ClientType {
        get {
            return _clientType;
        }
        set {
            if (value == OpenIddictConstants.ClientTypes.Public)
            {
                AllowClientCredentialsFlow = false;
                AllowDeviceEndpoint = false;
            }
            _clientType = value;
        }
    }

    public string ClientSecret { get; set; }

    public string ConsentType { get; set; }

    public string ClientUri { get; set; }

    public string LogoUri { get; set; }

    public string ExtensionGrantTypes { get; set; }

    public string PostLogoutRedirectUris { get; set; }

    public string RedirectUris { get; set; }

    public bool AllowPasswordFlow { get; set; }

    public bool AllowClientCredentialsFlow { get; set; }

    public bool AllowAuthorizationCodeFlow { get; set; }

    public bool AllowRefreshTokenFlow { get; set; }

    public bool AllowHybridFlow { get; set; }

    public bool AllowImplicitFlow { get; set; }

    public bool AllowLogoutEndpoint { get; set; }

    public bool AllowDeviceEndpoint { get; set; }

    public HashSet<string> Scopes { get; set; }
}

public class ApplicationTokenLifetimeModalView
{
    public double? AccessTokenLifetime { get; set; }

    public double? AuthorizationCodeLifetime  { get; set; }

    public double? DeviceCodeLifetime  { get; set; }

    public double? IdentityTokenLifetime { get; set; }

    public double? RefreshTokenLifetime { get; set; }

    public double? UserCodeLifetime { get; set; }
}
