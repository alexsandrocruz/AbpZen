using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Volo.Abp.AuditLogging;

[Authorize(AbpAuditLoggingPermissions.AuditLogs.SettingManagement)]
public class AuditLogSettingsAppService : AuditLogsAppServiceBase, IAuditLogSettingsAppService
{
    protected ISettingManager SettingManager { get; }

    public AuditLogSettingsAppService(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    public virtual async Task<AuditLogSettingsDto> GetAsync()
    {
        return new AuditLogSettingsDto
        {
            IsExpiredDeleterEnabled =
                await SettingProvider.GetAsync<bool>(AuditLogSettingNames.IsExpiredDeleterEnabled),
            ExpiredDeleterPeriod = await SettingProvider.GetAsync<int>(AuditLogSettingNames.ExpiredDeleterPeriod)
        };
    }

    public virtual async Task UpdateAsync(AuditLogSettingsDto input)
    {
        await CheckExpiredDeletionSettingsAsync(input);

        await SettingManager.SetForCurrentTenantAsync(
            AuditLogSettingNames.IsExpiredDeleterEnabled,
            input.IsExpiredDeleterEnabled.ToString());

        await SettingManager.SetForCurrentTenantAsync(
            AuditLogSettingNames.ExpiredDeleterPeriod,
            input.IsExpiredDeleterEnabled ? input.ExpiredDeleterPeriod.ToString() : "0");
    }

    public virtual async Task<AuditLogGlobalSettingsDto> GetGlobalAsync()
    {
        CheckTenantAvailable();

        return new AuditLogGlobalSettingsDto
        {
            IsPeriodicDeleterEnabled = (await SettingManager.GetOrNullGlobalAsync(AuditLogSettingNames.IsPeriodicDeleterEnabled))
                ?.To<bool>() ?? false,
            IsExpiredDeleterEnabled =
                (await SettingManager.GetOrNullGlobalAsync(AuditLogSettingNames.IsExpiredDeleterEnabled))
                ?.To<bool>() ?? false,
            ExpiredDeleterPeriod =
                (await SettingManager.GetOrNullGlobalAsync(AuditLogSettingNames.ExpiredDeleterPeriod))
                ?.To<int>() ?? 0,
        };
    }

    public virtual async Task UpdateGlobalAsync(AuditLogGlobalSettingsDto input)
    {
        CheckTenantAvailable();
        await CheckGlobalSettingsAsync(input);

        await SettingManager.SetGlobalAsync(
            AuditLogSettingNames.IsPeriodicDeleterEnabled,
            input.IsPeriodicDeleterEnabled.ToString());

        await SettingManager.SetGlobalAsync(
            AuditLogSettingNames.IsExpiredDeleterEnabled,
            input.IsExpiredDeleterEnabled.ToString());

        await SettingManager.SetGlobalAsync(
            AuditLogSettingNames.ExpiredDeleterPeriod,
            input.IsExpiredDeleterEnabled ? input.ExpiredDeleterPeriod.ToString() : "0");
    }

    protected virtual ValueTask CheckExpiredDeletionSettingsAsync(AuditLogSettingsDto input)
    {
        if (input.IsExpiredDeleterEnabled && input.ExpiredDeleterPeriod <= 0 ||
            !input.IsExpiredDeleterEnabled && input.ExpiredDeleterPeriod > 0)
        {
            throw new UserFriendlyException(message: L["InvalidAuditLogDeletionSettings"]);
        }

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask CheckGlobalSettingsAsync(AuditLogGlobalSettingsDto input)
    {
        if (!input.IsPeriodicDeleterEnabled)
        {
            input.IsExpiredDeleterEnabled = false;
            input.ExpiredDeleterPeriod = 0;
        }
        
        CheckExpiredDeletionSettingsAsync(input);

        return ValueTask.CompletedTask;
    }

    protected virtual void CheckTenantAvailable()
    {
        if (CurrentTenant.IsAvailable)
        {
            throw new AbpAuthorizationException();
        }
    }
}