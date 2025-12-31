using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.Saas.Editions;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Tenants;

namespace Volo.Saas.Host;

[Authorize(SaasHostPermissions.Tenants.Default)]
public class TenantAppService : SaasHostAppServiceBase, ITenantAppService
{
    protected IEditionRepository EditionRepository { get; }
    protected IDataSeeder DataSeeder { get; }
    protected ILocalEventBus LocalEventBus { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected ITenantRepository TenantRepository { get; }
    protected ITenantManager TenantManager { get; }
    protected AbpDbConnectionOptions DbConnectionOptions { get; }
    protected IConnectionStringChecker ConnectionStringChecker { get; }

    public TenantAppService(
        ITenantRepository tenantRepository,
        IEditionRepository editionRepository,
        ITenantManager tenantManager,
        IDataSeeder dataSeeder,
        ILocalEventBus _localEventBus,
        IDistributedEventBus distributedEventBus,
        IOptions<AbpDbConnectionOptions> dbConnectionOptions,
        IConnectionStringChecker connectionStringChecker)
    {
        EditionRepository = editionRepository;
        DataSeeder = dataSeeder;
        LocalEventBus = _localEventBus;
        DistributedEventBus = distributedEventBus;
        DbConnectionOptions = dbConnectionOptions.Value;
        TenantRepository = tenantRepository;
        TenantManager = tenantManager;
        ConnectionStringChecker = connectionStringChecker;
    }

    public virtual async Task<SaasTenantDto> GetAsync(Guid id)
    {
        var tenant = ObjectMapper.Map<Tenant, SaasTenantDto>(
            await TenantRepository.GetAsync(id)
        );

        if (tenant.EditionEndDateUtc <= DateTime.UtcNow)
        {
            tenant.EditionId = null;
        }

        if (tenant.EditionId.HasValue)
        {
            var edition = await EditionRepository.GetAsync(tenant.EditionId.Value);
            tenant.EditionName = edition.DisplayName;
        }

        return tenant;
    }

    public virtual async Task<PagedResultDto<SaasTenantDto>> GetListAsync(GetTenantsInput input)
    {
        var count = await TenantRepository.GetCountAsync(
            input.Filter,
            input.EditionId,
            input.ExpirationDateMin,
            input.ExpirationDateMax,
            input.ActivationState,
            input.ActivationEndDateMin,
            input.ActivationEndDateMin);

        var list = await TenantRepository.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.Filter,
            includeDetails: true,
            input.EditionId,
            input.ExpirationDateMin,
            input.ExpirationDateMax,
            input.ActivationState,
            input.ActivationEndDateMin,
            input.ActivationEndDateMax
        );

        var tenantDtos = ObjectMapper.Map<List<Tenant>, List<SaasTenantDto>>(list);

        var editions = input.GetEditionNames ? await EditionRepository.GetListAsync() : null;

        foreach (var tenant in tenantDtos)
        {
            var isEditionExpired = tenant.EditionEndDateUtc <= DateTime.UtcNow;

            if (isEditionExpired)
            {
                tenant.EditionId = null;
            }

            if (input.GetEditionNames && !isEditionExpired)
            {
                var edition = editions?.FirstOrDefault(e => e.Id == tenant.EditionId);
                tenant.EditionName = edition?.DisplayName;
            }
        }

        return new PagedResultDto<SaasTenantDto>(
            count,
            tenantDtos
        );
    }

    [Authorize(SaasHostPermissions.Tenants.Create)]
    public virtual async Task<SaasTenantDto> CreateAsync(SaasTenantCreateDto input)
    {
        input.ConnectionStrings = await NormalizedConnectionStringsAsync(input.ConnectionStrings);

        Tenant tenant = null;

        async Task CreateTenantAsync()
        {
            tenant = await TenantManager.CreateAsync(input.Name, input.EditionId);

            if (!input.ConnectionStrings.Default.IsNullOrWhiteSpace())
            {
                tenant.SetDefaultConnectionString(input.ConnectionStrings.Default);
            }

            if (input.ConnectionStrings.Databases != null)
            {
                foreach (var database in input.ConnectionStrings.Databases)
                {
                    tenant.SetConnectionString(database.DatabaseName, database.ConnectionString);
                }
            }

            input.MapExtraPropertiesTo(tenant);

            tenant.SetActivationState(input.ActivationState);
            if (tenant.ActivationState == TenantActivationState.ActiveWithLimitedTime)
            {
                tenant.SetActivationEndDate(input.ActivationEndDate);
            }
            /* Auto saving to ensure TenantCreatedEto handler can get the tenant! */
            await TenantRepository.InsertAsync(tenant, autoSave: true);
        }

        if (input.ConnectionStrings.Default.IsNullOrWhiteSpace() &&
            input.ConnectionStrings.Databases.IsNullOrEmpty())
        {
            /* Creating the tenant in the current UOW */
            await CreateTenantAsync();
        }
        else
        {
            /* Creating the tenant in a separate UOW to ensure it is created
             * before creating the database.
             * TODO: We should remove inner UOW once https://github.com/abpframework/abp/issues/6126 is done
             */
            using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
            {
                await CreateTenantAsync();
                await uow.CompleteAsync();
            }
        }

        await DistributedEventBus.PublishAsync(
            new TenantCreatedEto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Properties =
                {
                        {"AdminEmail", input.AdminEmailAddress},
                        {"AdminPassword", input.AdminPassword}
                }
            }
        );

        return ObjectMapper.Map<Tenant, SaasTenantDto>(tenant);
    }

    [Authorize(SaasHostPermissions.Tenants.Update)]
    public virtual async Task<SaasTenantDto> UpdateAsync(Guid id, SaasTenantUpdateDto input)
    {
        var tenant = await TenantRepository.GetAsync(id);

        tenant.EditionId = input.EditionId;
        await TenantManager.ChangeNameAsync(tenant, input.Name);
        tenant.SetActivationState(input.ActivationState);
        tenant.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        input.MapExtraPropertiesTo(tenant);

        if (tenant.ActivationState == TenantActivationState.ActiveWithLimitedTime)
        {
            tenant.SetActivationEndDate(input.ActivationEndDate);
        }
        await TenantRepository.UpdateAsync(tenant);

        return ObjectMapper.Map<Tenant, SaasTenantDto>(tenant);
    }

    [Authorize(SaasHostPermissions.Tenants.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var tenant = await TenantRepository.FindAsync(id);
        if (tenant == null)
        {
            return;
        }

        await TenantRepository.DeleteAsync(tenant);
    }

    [Authorize(SaasHostPermissions.Tenants.ManageConnectionStrings)]
    public virtual Task<SaasTenantDatabasesDto> GetDatabasesAsync()
    {
        var dto = new SaasTenantDatabasesDto()
        {
            Databases = DbConnectionOptions.Databases.
                Where(x => x.Value.IsUsedByTenants).
                Select(x => x.Key).
                ToList()
        };

        return Task.FromResult(dto);
    }

    [Authorize(SaasHostPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task<SaasTenantConnectionStringsDto> GetConnectionStringsAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);

        var dto = new SaasTenantConnectionStringsDto()
        {
            Default = tenant.FindDefaultConnectionString(),
            Databases = new List<SaasTenantDatabaseConnectionStringsDto>()
        };

        foreach (var database in DbConnectionOptions.Databases.Where(x => x.Value.IsUsedByTenants))
        {
            dto.Databases.Add(new SaasTenantDatabaseConnectionStringsDto
            {
                DatabaseName = database.Key,
                ConnectionString = tenant.FindConnectionString(database.Key)
            });
        }

        return dto;
    }

    [Authorize(SaasHostPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task UpdateConnectionStringsAsync(Guid id, SaasTenantConnectionStringsDto input)
    {
        input = await NormalizedConnectionStringsAsync(input);

        Tenant tenant;

        var etos = new List<TenantConnectionStringUpdatedEto>();

        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            tenant = await TenantRepository.GetAsync(id);
            var tenantDefaultConnectionString = tenant.FindDefaultConnectionString();
            if (input.Default != tenantDefaultConnectionString)
            {
                etos.Add(new TenantConnectionStringUpdatedEto
                {
                    Id = tenant.Id,
                    Name = tenant.Name,
                    ConnectionStringName = ConnectionStrings.DefaultConnectionStringName,
                    OldValue = tenantDefaultConnectionString,
                    NewValue = input.Default
                });

                if (input.Default.IsNullOrWhiteSpace())
                {
                    tenant.RemoveDefaultConnectionString();
                }
                else
                {
                    tenant.SetDefaultConnectionString(input.Default);
                }
            }

            if (input.Databases != null)
            {
                //delete
                var toBeDeleted = tenant.ConnectionStrings
                    .Where(x => x.Name != ConnectionStrings.DefaultConnectionStringName && input.Databases.All(d => x.Name != d.DatabaseName))
                    .ToList();
                etos.AddRange(toBeDeleted.Select(connectionString => new TenantConnectionStringUpdatedEto
                {
                    Id = tenant.Id,
                    Name = tenant.Name,
                    ConnectionStringName = connectionString.Name,
                    OldValue = connectionString.Value,
                    NewValue = null
                }));
                tenant.ConnectionStrings.RemoveAll(x => toBeDeleted.Any(y => x.Name == y.Name));

                foreach (var database in input.Databases)
                {
                    var oldConnectionString = tenant.FindConnectionString(database.DatabaseName);
                    etos.Add(new TenantConnectionStringUpdatedEto
                    {
                        Id = tenant.Id,
                        Name = tenant.Name,
                        ConnectionStringName = database.DatabaseName,
                        OldValue = oldConnectionString,
                        NewValue = database.ConnectionString
                    });
                    tenant.SetConnectionString(database.DatabaseName, database.ConnectionString);
                }
            }

            await TenantRepository.UpdateAsync(tenant);

            await uow.CompleteAsync();
        }

        if (etos.Count > 0)
        {
            await LocalEventBus.PublishAsync(new TenantChangedEvent(tenant.Id, tenant.NormalizedName));
        }

        foreach (var eto in etos)
        {
            await DistributedEventBus.PublishAsync(eto);
        }
    }

    protected virtual Task<SaasTenantConnectionStringsDto> NormalizedConnectionStringsAsync(SaasTenantConnectionStringsDto input)
    {
        if (input == null)
        {
            input = new SaasTenantConnectionStringsDto();
        }
        else if (!input.Databases.IsNullOrEmpty())
        {
            input.Databases = input.Databases
                .Where(x => DbConnectionOptions.Databases.Any(d => d.Key == x.DatabaseName && d.Value.IsUsedByTenants))
                .Where(x => !x.ConnectionString.IsNullOrWhiteSpace())
                .GroupBy(x => x.DatabaseName)
                .Select(x => x.First())
                .ToList();
        }

        return Task.FromResult(input);
    }

    [Authorize(SaasHostPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task ApplyDatabaseMigrationsAsync(Guid id)
    {
        await DistributedEventBus.PublishAsync(
            new ApplyDatabaseMigrationsEto
            {
                TenantId = id,
                DatabaseName = ConnectionStrings.DefaultConnectionStringName
            }
        );

        foreach (var databaseInfo in DbConnectionOptions.Databases.Values)
        {
            if (!databaseInfo.IsUsedByTenants)
            {
                continue;
            }

            await DistributedEventBus.PublishAsync(
                new ApplyDatabaseMigrationsEto
                {
                    TenantId = id,
                    DatabaseName = databaseInfo.DatabaseName
                }
            );
        }
    }

    public virtual async Task<List<EditionLookupDto>> GetEditionLookupAsync()
    {
        var editions = await EditionRepository.GetListAsync();

        return ObjectMapper.Map<List<Edition>, List<EditionLookupDto>>(editions);
    }

    public virtual async Task<bool> CheckConnectionStringAsync(string connectionString)
    {
        return (await ConnectionStringChecker.CheckAsync(connectionString)).Connected;
    }

    [Authorize(SaasHostPermissions.Tenants.SetPassword)]
    public virtual async Task SetPasswordAsync(Guid id, SaasTenantSetPasswordDto input)
    {
        await DistributedEventBus.PublishAsync(
            new UserPasswordChangeRequestedEto
            {
                TenantId = id,
                UserName = input.Username,
                Password = input.Password
            }
        );
    }
}
