using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.AuditLogging.Blazor.Pages.AuditLogging.Components;
public partial class AuditLogSettingGroup
{
    public AuditLogSettingsDto AuditLogSettings { get; } = new();

    public AuditLogGlobalSettingsDto? AuditLogGlobalSettings { get; protected set; }

    [Inject] protected IAuditLogSettingsAppService AuditLogSettingsAppService { get; set; }

    protected Validations AuditLogSettingsValidation;

    protected Validations AuditLogGlobalSettingsValidation;

    protected string selectedTab = "AuditLogSettingsGlobal";

    protected override async Task OnInitializedAsync()
    {
        await GetSettingsAsync();
    }

    private async Task GetSettingsAsync()
    {
        try
        {
            var auditLogSettings = await AuditLogSettingsAppService.GetAsync();

            AuditLogSettings.ExpiredDeleterPeriod = auditLogSettings.ExpiredDeleterPeriod;
            AuditLogSettings.IsExpiredDeleterEnabled = auditLogSettings.IsExpiredDeleterEnabled;

            if (!CurrentTenant.IsAvailable)
            {
                var auditLogGlobalSettings = await AuditLogSettingsAppService.GetGlobalAsync();
                
                AuditLogGlobalSettings ??= new AuditLogGlobalSettingsDto();

                AuditLogGlobalSettings.IsExpiredDeleterEnabled = auditLogGlobalSettings.IsExpiredDeleterEnabled;
                AuditLogGlobalSettings.IsPeriodicDeleterEnabled = auditLogGlobalSettings.IsPeriodicDeleterEnabled;
                AuditLogGlobalSettings.ExpiredDeleterPeriod = auditLogGlobalSettings.ExpiredDeleterPeriod;

            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected async Task SaveAuditLogSettingsAsync()
    {
        try
        {
            if (!await AuditLogSettingsValidation.ValidateAll())
            {
                return;
            }

            if (!AuditLogSettings.IsExpiredDeleterEnabled)
            {
                AuditLogSettings.ExpiredDeleterPeriod = 0;
            }

            await AuditLogSettingsAppService.UpdateAsync(AuditLogSettings);

            await Notify.Success(L["SavedSuccessfully"]);

            await GetSettingsAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected async Task SaveAuditLogGlobalSettingsAsync()
    {
        try
        {
            if (!await AuditLogGlobalSettingsValidation.ValidateAll())
            {
                return;
            }

            if (!AuditLogGlobalSettings.IsExpiredDeleterEnabled)
            {
                AuditLogGlobalSettings.ExpiredDeleterPeriod = 0;
            }

            await AuditLogSettingsAppService.UpdateGlobalAsync(AuditLogGlobalSettings);

            await Notify.Success(L["SavedSuccessfully"]);

            await GetSettingsAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}
