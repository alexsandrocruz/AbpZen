using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Volo.Abp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.FeatureManagement.Blazor.Components;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Users;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Localization;
using Microsoft.Extensions.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.Validation;

namespace Volo.Saas.Host.Blazor.Pages.Saas.Host;

public partial class TenantManagement
{
    [Inject] protected IEditionAppService EditionAppService { get; set; }
    [Inject] protected ITenantAppService TenantAppService { get; set; }
    [Inject] protected IJSRuntime JSRuntime { get; set; }
    [Inject] protected IOptions<SaasHostBlazorOptions> Options { get; set; }
    [Inject] protected IStringLocalizer<AbpUiResource> AbpUiLocalizer { get; set; }

    protected const string FeatureProviderName = "T";

    protected bool HasManageFeaturesPermission;
    protected bool HasManageSetPasswordPermission;
    protected bool HasManageConnectionStringsPermission;
    protected bool HasImpersonationPermission;
    protected string ManageFeaturesPolicyName;
    protected string ManageSetPasswordPolicyName;
    protected string ManageConnectionStringsPolicyName;
    protected string ImpersonationPolicyName;

    protected FeatureManagementModal FeatureManagementModal;

    protected Modal ManageConnectionStringModal;

    protected TenantConnectionStringsModel ConnectionStrings;

    protected Modal TenantImpersonationModal;

    protected string DefaultAdminUserName = "admin";

    protected string TenantImpersonationReturnUrl = "/Saas/Host/Tenants";
    protected string CurrentImpersonationTenantName;

    protected List<EditionDto> Editions;

    protected Validations ManageConnectionStringValidations;

    protected bool CreateModalUseSharedDatabase = true;
    protected bool UseModuleSpecificDatabase = false;

    protected string DatabaseName { get; set; }
    protected string ConnectionString { get; set; }
    protected List<NameValue> DatabaseSelectListItems { get; set; } = new List<NameValue>();

    protected string SelectedTab = "info";
    protected PageToolbar Toolbar { get; } = new();

    protected List<TableColumn> TenantManagementTableColumns => TableColumns.Get<TenantManagement>();

    protected bool ShowAdvancedFilters { get; set; }

    protected Modal ChangePasswordModal;
    protected bool ShowPassword { get; set; }

    protected TextRole ChangePasswordTextRole { get; set; }

    protected ChangeUserPasswordViewModel ChangePasswordModel { get; set; }

    protected AdvancedFilterInput AdvancedFilterInput { get; set; } = new();

    public TenantManagement()
    {
        ObjectMapperContext = typeof(SaasHostBlazorModule);
        LocalizationResource = typeof(SaasResource);

        CreatePolicyName = SaasHostPermissions.Tenants.Create;
        UpdatePolicyName = SaasHostPermissions.Tenants.Update;
        DeletePolicyName = SaasHostPermissions.Tenants.Delete;
        ManageFeaturesPolicyName = SaasHostPermissions.Tenants.ManageFeatures;
        ManageSetPasswordPolicyName = SaasHostPermissions.Tenants.SetPassword;
        ManageConnectionStringsPolicyName = SaasHostPermissions.Tenants.ManageConnectionStrings;
        ImpersonationPolicyName = SaasHostPermissions.Tenants.Impersonation;

        ConnectionStrings = new TenantConnectionStringsModel();
        ChangePasswordModel = new ChangeUserPasswordViewModel();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (await AuthorizationService.IsGrantedAsync(SaasHostPermissions.Editions.Default))
        {
            Editions ??= await EditionAppService.GetAllListAsync();
        }
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();
        HasManageFeaturesPermission = await AuthorizationService.IsGrantedAsync(ManageFeaturesPolicyName);
        HasManageSetPasswordPermission = await AuthorizationService.IsGrantedAsync(ManageSetPasswordPolicyName);
        HasManageConnectionStringsPermission = await AuthorizationService.IsGrantedAsync(ManageConnectionStringsPolicyName);
        HasImpersonationPermission = await AuthorizationService.IsGrantedAsync(ImpersonationPolicyName);
    }

    protected override async Task OpenCreateModalAsync()
    {
        CreateModalUseSharedDatabase = true;
        UseModuleSpecificDatabase = false;

        if (await AuthorizationService.IsGrantedAsync(SaasHostPermissions.Editions.Default))
        {
            Editions ??= await EditionAppService.GetAllListAsync();
        }

        ConnectionStrings = new TenantConnectionStringsModel
        {
            UseSharedDatabase = true,
            Default = null,
            Databases = (await AppService.GetDatabasesAsync()).Databases.Select(x =>
                new TenantDatabaseConnectionStringsModel()
                {
                    DatabaseName = x,
                    ConnectionString = null
                }).ToList()
        };

        SelectedTab = "info";
        DatabaseName = GetSelectValue().FirstOrDefault()?.Name;

        await base.OpenCreateModalAsync();
    }

    protected virtual void ValidateNewEntityActivationEndDate(ValidatorEventArgs e)
    {
        e.Status = NewEntity.ActivationState == TenantActivationState.ActiveWithLimitedTime && NewEntity.ActivationEndDate == null
            ? ValidationStatus.Error
            : ValidationStatus.Success;

        

        e.ErrorText = e.Status == ValidationStatus.Error ? L["The {0} field is required.", L[$"DisplayName:{nameof(NewEntity.ActivationEndDate)}"]] : null;
    }

    protected override Task CreateEntityAsync()
    {
        if (NewEntity.EditionId == Guid.Empty)
        {
            NewEntity.EditionId = null;
        }

        if (CreateModalUseSharedDatabase)
        {
            NewEntity.ConnectionStrings = null;
        }
        else
        {
            ConnectionStrings.Databases.RemoveAll(x => x.ConnectionString.IsNullOrWhiteSpace());
            NewEntity.ConnectionStrings = ObjectMapper.Map<TenantConnectionStringsModel, SaasTenantConnectionStringsDto>(ConnectionStrings);
        }

        return base.CreateEntityAsync();
    }

    protected override async Task OpenEditModalAsync(SaasTenantDto entity)
    {
        if (await AuthorizationService.IsGrantedAsync(SaasHostPermissions.Editions.Default))
        {
            Editions ??= await EditionAppService.GetAllListAsync();
        }

        await base.OpenEditModalAsync(entity);
    }

    protected virtual void ValidateEditingEntityActivationEndDate(ValidatorEventArgs e)
    {
        e.Status = EditingEntity.ActivationState == TenantActivationState.ActiveWithLimitedTime && EditingEntity.ActivationEndDate == null
            ? ValidationStatus.Error
            : ValidationStatus.Success;

        e.ErrorText = e.Status == ValidationStatus.Error ? L["The {0} field is required.", L[$"DisplayName:{nameof(EditingEntity.ActivationEndDate)}"]] : null;
    }
    
    protected virtual void ValidateDefaultConnectionString(ValidatorEventArgs e) 
    {
        e.Status = !ConnectionStrings.UseSharedDatabase && ConnectionStrings.Default.IsNullOrWhiteSpace()
            ? ValidationStatus.Error
            : ValidationStatus.Success;
        
        e.ErrorText = e.Status == ValidationStatus.Error ? L["The {0} field is required.", L[$"DisplayName:{nameof(ConnectionStrings.Default)}"]] : null;
    }

    protected override Task UpdateEntityAsync()
    {
        if (EditingEntity.EditionId == Guid.Empty)
        {
            EditingEntity.EditionId = null;
        }

        return base.UpdateEntityAsync();
    }

    protected virtual async Task OpenEditConnectionStringModalAsync(SaasTenantDto entity)
    {
        ConnectionStrings = ObjectMapper.Map<SaasTenantConnectionStringsDto, TenantConnectionStringsModel>(await AppService.GetConnectionStringsAsync(entity.Id));

        ConnectionStrings.Id = entity.Id;
        ConnectionStrings.TenantName = entity.Name;
        ConnectionStrings.UseSharedDatabase = ConnectionStrings.Default.IsNullOrWhiteSpace() &&
                                              (ConnectionStrings.Databases.IsNullOrEmpty() ||
                                               ConnectionStrings.Databases.All(x => x.ConnectionString.IsNullOrWhiteSpace()));
        UseModuleSpecificDatabase = ConnectionStrings.Databases.Any(x => !x.ConnectionString.IsNullOrEmpty());

        DatabaseName = GetSelectValue().FirstOrDefault()?.Name;
        await ManageConnectionStringModal.Show();
    }

    protected virtual void OnSelectedTabChanged(string name)
    {
        SelectedTab = name;
    }

    protected virtual async Task OnEditionChangedAsync(Guid editionId)
    {
        AdvancedFilterInput.EditionId = editionId;
        GetListInput.EditionId = editionId != Guid.Empty ? editionId : null;
        await base.SearchEntitiesAsync();
    }

    protected virtual async Task OnExpirationDatesChangedAsync(IReadOnlyList<DateTime?> dates)
    {
        if (dates != null && dates.Count == 2)
        {
            GetListInput.ExpirationDateMin = dates.Min();
            GetListInput.ExpirationDateMax = dates.Max();
        }
        else if (dates != null && dates.Count == 1)
        {
            GetListInput.ExpirationDateMin = dates[0];
            GetListInput.ExpirationDateMax = null;
        }
        else
        {
            GetListInput.ExpirationDateMin = null;
            GetListInput.ExpirationDateMax = null;
        }
        await base.SearchEntitiesAsync();
    }

    protected virtual async Task OnActivationEndDatesChangedAsync(IReadOnlyList<DateTime?> dates)
    {
        if (dates != null && dates.Count == 2)
        {
            GetListInput.ActivationEndDateMin = dates.Min();
            var max = dates.Max();
            GetListInput.ActivationEndDateMax =  max.Value.ClearTime().AddDays(1).AddSeconds(-1);
        }
        else if (dates != null && dates.Count == 1)
        {
            GetListInput.ActivationEndDateMin = dates[0];
            GetListInput.ActivationEndDateMax = null;
        }
        else
        {
            GetListInput.ActivationEndDateMin = null;
            GetListInput.ActivationEndDateMax = null;
        }
        await base.SearchEntitiesAsync();
    }

    protected virtual async Task OnActivationStateChangedAsync(int state)
    {
        AdvancedFilterInput.ActivationState = state;
        GetListInput.ActivationState = state == -1 ? null : (TenantActivationState?)state;
        await base.SearchEntitiesAsync();
    }

    protected virtual async Task CloseEditConnectionStringModal()
    {
        await ManageConnectionStringModal.Hide();
    }

    protected virtual async Task CloseTenantImpersonationModal()
    {
        await TenantImpersonationModal.Hide();
    }

    protected async Task CloseChangePasswordModalAsync()
    {
        ChangePasswordModel = new ChangeUserPasswordViewModel();
        await ChangePasswordModal.Close(CloseReason.None);
    }

    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual List<NameValue> GetSelectValue()
    {
        return ConnectionStrings.Databases
            .Where(x => x.ConnectionString.IsNullOrWhiteSpace())
            .Select(x => new NameValue(x.DatabaseName, x.DatabaseName))
            .ToList();
    }

    protected virtual void AddDatabaseConnectionString()
    {
        var database = ConnectionStrings.Databases.FirstOrDefault(x => x.DatabaseName == DatabaseName);
        if (database != null && !ConnectionString.IsNullOrWhiteSpace())
        {
            database.ConnectionString = ConnectionString;
            ConnectionString = null;
            DatabaseName = GetSelectValue().FirstOrDefault()?.Name;
        }
    }

    protected virtual async Task CheckDatabaseConnectionStringAsync(string connectionString)
    {
        if (connectionString.IsNullOrWhiteSpace())
        {
            return;
        }

        try
        {
            if (await AppService.CheckConnectionStringAsync(connectionString))
            {
                await Notify.Success(L["ValidConnectionString"]);
            }
            else
            {
                await Notify.Success(L["InvalidConnectionString"]);
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual void RemoveDatabaseConnectionString(string databaseName)
    {
        var database = ConnectionStrings.Databases.FirstOrDefault(x => x.DatabaseName == databaseName);
        if (database != null)
        {
            database.ConnectionString = null;
            DatabaseName = GetSelectValue().FirstOrDefault()?.Name;
        }
    }

    protected virtual async Task UpdateConnectionStringAsync()
    {
        try
        {
            await CheckPolicyAsync(ManageConnectionStringsPolicyName);

            if (!await ManageConnectionStringValidations.ValidateAll())
            {
                return;
            }

            if (ConnectionStrings.UseSharedDatabase)
            {
                await AppService.UpdateConnectionStringsAsync(ConnectionStrings.Id, new SaasTenantConnectionStringsDto()
                {
                    Databases = new List<SaasTenantDatabaseConnectionStringsDto>()
                });
            }
            else
            {
                var input = ObjectMapper.Map<TenantConnectionStringsModel, SaasTenantConnectionStringsDto>(ConnectionStrings);
                input.Databases.RemoveAll(x => x.ConnectionString.IsNullOrWhiteSpace());
                await AppService.UpdateConnectionStringsAsync(ConnectionStrings.Id, input);
            }

            await base.SearchEntitiesAsync();
            await ManageConnectionStringModal.Hide();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task ApplyDatabaseMigrationsAsync(SaasTenantDto entity)
    {
        try
        {
            await AppService.ApplyDatabaseMigrationsAsync(entity.Id);
            await Notify.Info(L["DatabaseMigrationQueuedAndWillBeApplied"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Saas"]));
        BreadcrumbItems.Add(new Abp.BlazoriseUI.BreadcrumbItem(L["Tenants"]));

        return ValueTask.CompletedTask;
    }

    protected override string GetDeleteConfirmationMessage(SaasTenantDto entity)
    {
        return string.Format(L["TenantDeletionConfirmationMessage"], entity.Name);
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<TenantManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data)=> HasUpdatePermission,
                        Clicked = async (data) => await OpenEditModalAsync(data.As<SaasTenantDto>())
                    },
                    new EntityAction
                    {
                        Text = L["ConnectionStrings"],
                        Visible = (data)=> HasManageConnectionStringsPermission,
                        Clicked = async (data) => await OpenEditConnectionStringModalAsync(data.As<SaasTenantDto>())
                    },
                    new EntityAction
                    {
                        Text = L["ApplyDatabaseMigrations"],
                        Clicked = async (data) => await ApplyDatabaseMigrationsAsync(data.As<SaasTenantDto>()),
                        Visible = (data)=>
                        {
                            var tenant = data.As<SaasTenantDto>();
                            return tenant.HasDefaultConnectionString && HasManageConnectionStringsPermission;
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Features"],
                        Visible = (data)=> HasManageFeaturesPermission,
                        Clicked = async (data) => await FeatureManagementModal.OpenAsync(FeatureProviderName, data.As<SaasTenantDto>().Id.ToString(), data.As<SaasTenantDto>().Name)
                    },
                    new EntityAction {
                        Text = L["SetPassword"],
                        Visible = (data) => HasManageSetPasswordPermission,
                        Clicked = async (data) =>
                        {
                            ChangePasswordTextRole = TextRole.Password;

                            ChangePasswordModel = new ChangeUserPasswordViewModel {
                                Id = data.As<SaasTenantDto>().Id,
                                TenantName = data.As<SaasTenantDto>().Name,
                                NewPassword = string.Empty,
                                UserName = TenantConsts.Username
                            };

                            await ChangePasswordModal.Show();
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data)=> HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<SaasTenantDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<SaasTenantDto>())
                    }
            });

        if (Options.Value.EnableTenantImpersonation && CurrentUser.FindImpersonatorUserId() == null)
        {
            EntityActions.Get<TenantManagement>().InsertBefore(x => x.Text == L["Delete"], new EntityAction
            {
                Text = L["LoginWithThisTenant"],
                Visible = (data) => (data.As<SaasTenantDto>().ActivationState == TenantActivationState.Active
                                        || (data.As<SaasTenantDto>().ActivationState == TenantActivationState.ActiveWithLimitedTime && data.As<SaasTenantDto>().ActivationEndDate > DateTime.Now))
                                    && HasImpersonationPermission,
                Clicked = async (data) =>
                {
                    CurrentImpersonationTenantName = data.As<SaasTenantDto>().Name;
                    await JSRuntime.InvokeVoidAsync("eval",
                        $"document.getElementById('ImpersonationTenantId').value = '{data.As<SaasTenantDto>().Id:D}'");
                    await TenantImpersonationModal.Show();
                }
            });
        }

        return base.SetEntityActionsAsync();
    }

    protected virtual async Task ImpersonationTenantAsync()
    {
        await JSRuntime.InvokeVoidAsync("eval", "document.getElementById('ImpersonationForm').submit()");
    }

    protected override async ValueTask SetTableColumnsAsync()
    {
        TenantManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<TenantManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["Name"],
                        Data = nameof(SaasTenantDto.Name),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["Edition"],
                        Data = nameof(SaasTenantDto.EditionName),
                        Sortable = false,
                    },
                    new TableColumn
                    {
                        Title  = L["EditionEndDateUtc"],
                        Data = nameof(SaasTenantDto.EditionEndDateUtc),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["ActivationState"],
                        Data = nameof(SaasTenantDto.ActivationState),
                        Sortable = true,
                        ValueConverter = (data) =>
                        {
                            var dto = data.As<SaasTenantDto>();
                            switch (dto.ActivationState)
                            {
                                case TenantActivationState.Active:
                                    return L["Enum:TenantActivationState.Active"];
                                case TenantActivationState.ActiveWithLimitedTime:
                                    return $"{L["Enum:TenantActivationState.ActiveWithLimitedTime"]} ({dto.ActivationEndDate})";
                                case TenantActivationState.Passive:
                                    return L["Enum:TenantActivationState.Passive"];
                            }
                            return null;
                        }
                    }
            });

        TenantManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(SaasModuleExtensionConsts.ModuleName,
            SaasModuleExtensionConsts.EntityNames.Tenant));

        await base.SetTableColumnsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewTenant"], OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected async Task ChangePasswordAsync()
    {
        try
        {
            await TenantAppService.SetPasswordAsync(ChangePasswordModel.Id, new SaasTenantSetPasswordDto()
            {
                Password = ChangePasswordModel.NewPassword,
                Username = ChangePasswordModel.UserName
            });
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception e)
        {
            await Message.Error(e.Message);
            return;
        }

        await CloseChangePasswordModalAsync();
    }

    protected void GenerateRandomPassword()
    {
        var rnd = new Random();
        var specials = "!*_#/+-.";
        var lowercase = "abcdefghjkmnpqrstuvwxyz";
        var uppercase = lowercase.ToUpper();
        var numbers = "23456789";

        var password =
            lowercase.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(4, 6)).JoinAsString("") +
            specials.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(1, 2)).JoinAsString("") +
            uppercase.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(2, 3)).JoinAsString("") +
            numbers.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(1, 2)).JoinAsString("");

        ChangePasswordModel.NewPassword = password.ToArray().OrderBy(x => rnd.Next()).JoinAsString("");
        ChangePasswordTextRole = TextRole.Text;
    }

    protected virtual void TogglePasswordVisibility()
    {
        ShowPassword = !ShowPassword;
    }

    protected virtual void ToggleChangePasswordVisibility()
    {
        ChangePasswordTextRole = ChangePasswordTextRole == TextRole.Password ? TextRole.Text : TextRole.Password;
    }
}

public class TenantConnectionStringsModel : ExtensibleObject
{
    public Guid Id { get; set; }
    
    public string TenantName { get; set; }

    public bool UseSharedDatabase { get; set; }

    [Required]
    [DynamicStringLength(typeof(TenantConnectionStringConsts),nameof(TenantConnectionStringConsts.MaxValueLength))]
    public string Default { get; set; }

    public List<TenantDatabaseConnectionStringsModel> Databases { get; set; }
}

public class TenantDatabaseConnectionStringsModel : ExtensibleObject
{
    public string DatabaseName { get; set; }

    [DynamicStringLength(typeof(TenantConnectionStringConsts),nameof(TenantConnectionStringConsts.MaxValueLength))]
    public string ConnectionString { get; set; }
}

public class ChangeUserPasswordViewModel
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string TenantName { get; set; }

    [DataType(DataType.Password)] public string NewPassword { get; set; }
}

public class AdvancedFilterInput
{
    public Guid EditionId { get; set; }
    public int ActivationState { get; set; } = -1;
}