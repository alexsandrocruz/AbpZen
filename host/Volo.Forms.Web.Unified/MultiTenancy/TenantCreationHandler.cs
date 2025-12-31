using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Forms.EntityFrameworkCore;

namespace Volo.Forms.MultiTenancy;

public class TenantCreationHandler : IDistributedEventHandler<TenantCreatedEto>, ITransientDependency
{
    private readonly ITenantStore _tenantStore;
    private readonly IDataSeeder _dataSeeder;
    private readonly ICurrentTenant _currentTenant;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly FormsDbContext _formsDbContext;
    private readonly ILogger<TenantCreationHandler> _logger;

    public TenantCreationHandler(
        ITenantStore tenantStore,
        IDataSeeder dataSeeder,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        FormsDbContext formsDbContext,
        ILogger<TenantCreationHandler> logger)
    {
        _tenantStore = tenantStore;
        _dataSeeder = dataSeeder;
        _currentTenant = currentTenant;
        _unitOfWorkManager = unitOfWorkManager;
        _formsDbContext = formsDbContext;
        _logger = logger;
    }

    public async Task HandleEventAsync(TenantCreatedEto eventData)
    {
        await MigrateAndSeedForTenantAsync(
            eventData.Id,
            eventData.Properties.GetOrDefault("AdminEmail") ?? "admin@admin.com",
            eventData.Properties.GetOrDefault("AdminPassword") ?? "123qwE*"
        );
    }

    private async Task MigrateAndSeedForTenantAsync(
        Guid tenantId,
        string adminEmail,
        string adminPassword)
    {
        try
        {
            using (_currentTenant.Change(tenantId))
            {
                // Create database tables if needed
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
                {
                    var tenantConfiguration = await _tenantStore.FindAsync(tenantId);
                    if (tenantConfiguration?.ConnectionStrings != null &&
                        !tenantConfiguration.ConnectionStrings.Default.IsNullOrWhiteSpace())
                    {
                        await _formsDbContext.Database.MigrateAsync();
                    }

                    await uow.CompleteAsync();
                }

                // Seed data
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                {
                    await _dataSeeder.SeedAsync(
                        new DataSeedContext(tenantId)
                            .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, adminEmail)
                            .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, adminPassword)
                    );

                    await uow.CompleteAsync();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
        }
    }
}