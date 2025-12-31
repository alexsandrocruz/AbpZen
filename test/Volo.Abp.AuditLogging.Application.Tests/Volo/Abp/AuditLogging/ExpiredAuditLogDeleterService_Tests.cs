using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Auditing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;
using Xunit;

namespace Volo.Abp.AuditLogging;

public sealed class ExpiredAuditLogDeleterService_Tests :
    AbpAuditLoggingTestBase<AbpAuditLoggingTestBaseModule>,
    IAsyncLifetime
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IAuditLogInfoToAuditLogConverter _auditLogInfoToAuditLogConverter;
    private readonly ExpiredAuditLogDeleterService _expiredAuditLogDeleterService;
    private readonly ISettingManager _settingManager;
    private readonly ICurrentTenant _currentTenant;
    private readonly IFeatureManager _featureManager;
    private readonly TenantConfiguration[] _tenantConfigurations;

    public ExpiredAuditLogDeleterService_Tests()
    {
        _auditLogRepository = GetRequiredService<IAuditLogRepository>();
        _auditLogInfoToAuditLogConverter = GetRequiredService<IAuditLogInfoToAuditLogConverter>();
        _expiredAuditLogDeleterService = GetRequiredService<ExpiredAuditLogDeleterService>();
        _settingManager = GetRequiredService<ISettingManager>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _featureManager = GetRequiredService<IFeatureManager>();
        _tenantConfigurations = GetRequiredService<IOptions<AbpDefaultTenantStoreOptions>>().Value.Tenants;
    }

    public async Task InitializeAsync()
    {
        await TenantAction(async () =>
        {
            await _auditLogRepository.InsertManyAsync(await CreateAuditLogs());
        });
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<IEnumerable<AuditLog>> CreateAuditLogs(int size = 100)
    {
        var list = new List<AuditLog>(size);

        for (int i = 0; i < size; i++)
        {
            list.Add(await _auditLogInfoToAuditLogConverter.ConvertAsync(new AuditLogInfo {
                UserId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06"),
                ExecutionTime = DateTime.Today.AddDays(-i),
                ExecutionDuration = 42,
                ClientIpAddress = "153.1.7.61",
                ClientName = "MyDesktop",
                BrowserInfo = "Chrome",
                Comments = ["First Comment", "Second Comment"],
                UserName = "Douglas",
                TenantId = _currentTenant.Id,
                EntityChanges = {
                    new EntityChangeInfo {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                        ChangeType = EntityChangeType.Created,
                        ChangeTime = DateTime.Now,
                        PropertyChanges = [
                            new EntityPropertyChangeInfo {
                                PropertyTypeFullName = typeof(string).FullName!,
                                PropertyName = "Name",
                                NewValue = "New value",
                                OriginalValue = null
                            }
                        ]
                    },
                    new EntityChangeInfo {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                        ChangeType = EntityChangeType.Created,
                        ChangeTime = DateTime.Now,
                        PropertyChanges = [
                            new EntityPropertyChangeInfo {
                                PropertyTypeFullName = typeof(string).FullName!,
                                PropertyName = "Name",
                                NewValue = "New value",
                                OriginalValue = null
                            }
                        ]
                    }
                }
            }));
        }

        return list;
    }

    private async Task TenantAction(
        Func<Task> action,
        bool includeHost = true,
        IEnumerable<TenantConfiguration> tenants = null)
    {
        tenants ??= _tenantConfigurations;

        if (includeHost)
        {
            using (_currentTenant.Change(null))
            {
                await action();
            }
        }

        foreach (var tenant in tenants)
        {
            using (_currentTenant.Change(tenant.Id))
            {
                await action();
            }
        }
    }

    [Fact]
    public async Task When_Global_Setting_Is_Enabled_It_Should_Delete_Expired_Audit_Logs()
    {
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.IsExpiredDeleterEnabled, bool.TrueString);
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.ExpiredDeleterPeriod, 1.ToString());

        await _expiredAuditLogDeleterService.DeleteAsync();

        await TenantAction(async () =>
        {
            (await _auditLogRepository.GetCountAsync()).ShouldBe(2);
            (await _auditLogRepository.GetCountAsync(startTime: DateTime.Today.AddDays(-1))).ShouldBe(2);
            (await _auditLogRepository.GetCountAsync(endTime: DateTime.Today.AddDays(-2))).ShouldBe(0);
        });
    }

    [Fact]
    public async Task When_Tenant_Disables_The_Expired_Deleter_It_Should_Not_Delete_Audit_Logs()
    {
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.IsExpiredDeleterEnabled, bool.TrueString);
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.ExpiredDeleterPeriod, 1.ToString());
        await TenantAction(async () =>
            {
                await _featureManager.SetForTenantAsync(_currentTenant.Id!.Value,
                    AbpAuditLoggingFeatures.SettingManagement,
                    bool.TrueString);
                await _settingManager.SetForCurrentTenantAsync(AuditLogSettingNames.IsExpiredDeleterEnabled,
                    bool.FalseString);
            },
            includeHost: false);

        await _expiredAuditLogDeleterService.DeleteAsync();

        (await _auditLogRepository.GetCountAsync()).ShouldBe(2);
        (await _auditLogRepository.GetCountAsync(startTime: DateTime.Today.AddDays(-1))).ShouldBe(2);
        (await _auditLogRepository.GetCountAsync(endTime: DateTime.Today.AddDays(-2))).ShouldBe(0);
        await TenantAction(async () =>
            {
                (await _auditLogRepository.GetCountAsync()).ShouldBe(100);
                (await _auditLogRepository.GetCountAsync(endTime: DateTime.Today)).ShouldBe(100);
            },
            includeHost: false);
    }

    [Fact]
    public async Task When_Some_Tenant_Disables_The_Expired_Deleter_It_Should_Not_Delete_Audit_Logs_For_Them()
    {
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.IsExpiredDeleterEnabled, bool.TrueString);
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.ExpiredDeleterPeriod, 5.ToString());
        await TenantAction(async () =>
            {
                await _featureManager.SetForTenantAsync(_currentTenant.Id!.Value,
                    AbpAuditLoggingFeatures.SettingManagement, bool.TrueString);
                await _settingManager.SetForCurrentTenantAsync(AuditLogSettingNames.IsExpiredDeleterEnabled,
                    bool.FalseString);
            },
            includeHost: false,
            tenants: _tenantConfigurations.Where(x => x.Name == "t-1"));

        await _expiredAuditLogDeleterService.DeleteAsync();

        await TenantAction(async () =>
            {
                (await _auditLogRepository.GetCountAsync()).ShouldBe(100);
                (await _auditLogRepository.GetCountAsync(endTime: DateTime.Today)).ShouldBe(100);
            },
            includeHost: false,
            tenants: _tenantConfigurations.Where(x => x.Name == "t-1"));
        await TenantAction(async () =>
            {
                (await _auditLogRepository.GetCountAsync()).ShouldBe(6);
                (await _auditLogRepository.GetCountAsync(startTime: DateTime.Today.AddDays(-5))).ShouldBe(6);
                (await _auditLogRepository.GetCountAsync(endTime: DateTime.Today.AddDays(-6))).ShouldBe(0);
            },
            tenants: _tenantConfigurations.Where(x => x.Name != "t-1"));
    }

    [Fact]
    public async Task When_Tenant_Has_Different_Period_It_Should_Apply_Its_Own_Setting()
    {
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.IsExpiredDeleterEnabled, bool.TrueString);
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.ExpiredDeleterPeriod, 5.ToString());
        await TenantAction(async () =>
            {
                await _featureManager.SetForTenantAsync(_currentTenant.Id!.Value,
                    AbpAuditLoggingFeatures.SettingManagement, bool.TrueString);
                await _settingManager.SetForCurrentTenantAsync(AuditLogSettingNames.IsExpiredDeleterEnabled,
                    bool.TrueString);
                await _settingManager.SetForCurrentTenantAsync(AuditLogSettingNames.ExpiredDeleterPeriod,
                    14.ToString());
            },
            includeHost: false,
            tenants: _tenantConfigurations.Where(x => x.Name == "t-1"));

        await _expiredAuditLogDeleterService.DeleteAsync();

        await TenantAction(async () =>
            {
                (await _auditLogRepository.GetCountAsync()).ShouldBe(15);
                (await _auditLogRepository.GetCountAsync(startTime: DateTime.Today.AddDays(-14))).ShouldBe(15);
                (await _auditLogRepository.GetCountAsync(endTime: DateTime.Today.AddDays(-15))).ShouldBe(0);
            },
            includeHost: false,
            tenants: _tenantConfigurations.Where(x => x.Name == "t-1"));
        await TenantAction(async () =>
            {
                (await _auditLogRepository.GetCountAsync()).ShouldBe(6);
                (await _auditLogRepository.GetCountAsync(startTime: DateTime.Today.AddDays(-5))).ShouldBe(6);
                (await _auditLogRepository.GetCountAsync(endTime: DateTime.Today.AddDays(-6))).ShouldBe(0);
            },
            tenants: _tenantConfigurations.Where(x => x.Name != "t-1"));
    }

    [Fact]
    public async Task When_Tenant_Has_Its_Own_Setting_But_Hasnt_Feature_It_Should_Apply_Global_Setting()
    {
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.IsExpiredDeleterEnabled, bool.TrueString);
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.ExpiredDeleterPeriod, 5.ToString());
        await TenantAction(async () =>
            {
                await _settingManager.SetForCurrentTenantAsync(AuditLogSettingNames.IsExpiredDeleterEnabled,
                    bool.FalseString);
            },
            includeHost: false,
            tenants: _tenantConfigurations.Where(x => x.Name == "t-1"));

        await _expiredAuditLogDeleterService.DeleteAsync();

        await TenantAction(async () =>
        {
            (await _auditLogRepository.GetCountAsync()).ShouldBe(6);
            (await _auditLogRepository.GetCountAsync(startTime: DateTime.Today.AddDays(-5))).ShouldBe(6);
            (await _auditLogRepository.GetCountAsync(endTime: DateTime.Today.AddDays(-6))).ShouldBe(0);
        });
    }

    [Fact]
    public async Task When_Cancellation_Token_Is_Set_It_Should_Stop_Deleting()
    {
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.IsExpiredDeleterEnabled, bool.TrueString);
        await _settingManager.SetGlobalAsync(AuditLogSettingNames.ExpiredDeleterPeriod, 5.ToString());

        var cancellationTokenProvider = GetRequiredService<ICancellationTokenProvider>();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            using (cancellationTokenProvider.Use(new CancellationToken(true)))
            {
                await _expiredAuditLogDeleterService.DeleteAsync();
            }
        });
        (await _auditLogRepository.GetCountAsync()).ShouldBe(100);
    }
}