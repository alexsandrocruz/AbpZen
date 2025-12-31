using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.AuditLogging;

public class ExpiredAuditLogDeleterService : ITransientDependency
{
    public ILogger<ExpiredAuditLogDeleterService> Logger { get; set; }

    protected IAbpDistributedLock DistributedLock { get; }
    protected ITenantStore TenantStore { get; }
    protected ISettingProvider SettingProvider { get; }
    protected ISettingManager SettingManager { get; }
    protected IFeatureChecker FeatureChecker { get; }
    protected IAuditLogRepository AuditLogRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IClock Clock { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    private bool _globalIsExpiredDeleterEnabled;
    private int _globalExpiredDeleterPeriod;

    public ExpiredAuditLogDeleterService(
        IAbpDistributedLock distributedLock,
        ITenantStore tenantStore,
        ISettingProvider settingProvider,
        ISettingManager settingManager,
        IFeatureChecker featureChecker,
        IAuditLogRepository auditLogRepository,
        ICurrentTenant currentTenant,
        IClock clock,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        Logger = NullLogger<ExpiredAuditLogDeleterService>.Instance;

        DistributedLock = distributedLock;
        TenantStore = tenantStore;
        SettingProvider = settingProvider;
        SettingManager = settingManager;
        FeatureChecker = featureChecker;
        AuditLogRepository = auditLogRepository;
        CurrentTenant = currentTenant;
        Clock = clock;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task DeleteAsync()
    {
        await using var handle = await DistributedLock.TryAcquireAsync(nameof(ExpiredAuditLogDeleterService));

        if (handle == null)
        {
            Logger.LogInformation(
                "The expired audit log deletion was skipped because there is already a lock for: {DistributedLockObject}",
                typeof(ExpiredAuditLogDeleterService));
            return;
        }

        Logger.LogInformation("Expired audit log deletion is started. Lock is acquired for {DistributedLockObject}",
            typeof(ExpiredAuditLogDeleterService));
        await FillGlobalSettings();

        using (CurrentTenant.Change(null))
        {
            await DeleteExpiredAuditLogsAsync();
        }

        foreach (var tenant in await TenantStore.GetListAsync())
        {
            CancellationTokenProvider.Token.ThrowIfCancellationRequested();
            using (CurrentTenant.Change(tenant.Id))
            {
                await DeleteExpiredAuditLogsAsync();
            }
        }

        Logger.LogInformation("Expired audit log deletion is completed. Lock is released for {DistributedLockObject}",
            typeof(ExpiredAuditLogDeleterService));
    }

    private async Task FillGlobalSettings()
    {
        _globalIsExpiredDeleterEnabled =
            bool.Parse(await SettingManager.GetOrNullGlobalAsync(AuditLogSettingNames.IsExpiredDeleterEnabled) ?? "false");

        if (_globalIsExpiredDeleterEnabled)
        {
            _globalExpiredDeleterPeriod =
                int.Parse(await SettingManager.GetOrNullGlobalAsync(AuditLogSettingNames.ExpiredDeleterPeriod));
        }
    }

    protected virtual async Task DeleteExpiredAuditLogsAsync()
    {
        var isFeatureEnable = await FeatureChecker.IsEnabledAsync(AbpAuditLoggingFeatures.SettingManagement);

        if (isFeatureEnable
                ? !(await SettingProvider.IsTrueAsync(AuditLogSettingNames.IsExpiredDeleterEnabled))
                : !_globalIsExpiredDeleterEnabled)
        {
            Logger.LogInformation("Expired audit log deletion was skipped for tenant: {TenantId}", CurrentTenant.Id);
            return;
        }

        var period = isFeatureEnable
            ? await SettingProvider.GetAsync<int>(AuditLogSettingNames.ExpiredDeleterPeriod)
            : _globalExpiredDeleterPeriod;
        var minDate = Clock.Now.Date.AddDays(-period);

        try
        {
            await AuditLogRepository.DeleteDirectAsync(
                x => x.TenantId == CurrentTenant.Id && x.ExecutionTime < minDate);
            Logger.LogInformation(
                "Expired audit logs have been successfully deleted for tenant: {TenantId}, covering dates up to less than {MinDate}",
                CurrentTenant.Id, minDate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to delete expired audit logs for tenant: {TenantId}", CurrentTenant.Id);
        }
    }
}