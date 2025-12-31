using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Saas.Tenants;

namespace Volo.Saas.Host;

public class SaasAppliedDatabaseMigrationsHandler : IDistributedEventHandler<AppliedDatabaseMigrationsEto>, ITransientDependency
{
    public ILogger<SaasAppliedDatabaseMigrationsHandler> Logger { get; set; }
    protected ITenantRepository TenantRepository { get; }
    protected IDistributedEventBus DistributedEventBus { get; }

    public SaasAppliedDatabaseMigrationsHandler(ITenantRepository tenantRepository, IDistributedEventBus distributedEventBus)
    {
        Logger = NullLogger<SaasAppliedDatabaseMigrationsHandler>.Instance;
        TenantRepository = tenantRepository;
        DistributedEventBus = distributedEventBus;
    }

    public virtual async Task HandleEventAsync(AppliedDatabaseMigrationsEto eventData)
    {
        Logger.LogDebug($"Handling {nameof(AppliedDatabaseMigrationsEto)} event. {nameof(eventData.DatabaseName)}: {eventData.DatabaseName}");
        if (eventData.TenantId != null)
        {
            Logger.LogDebug($"Skipping {nameof(AppliedDatabaseMigrationsEto)} event handling since it designed for host only.");
            return;
        }

        var tenants = await TenantRepository.GetListWithSeparateConnectionStringAsync(eventData.DatabaseName);
        foreach (var tenant in tenants)
        {
            Logger.LogDebug($"Publishing {nameof(ApplyDatabaseMigrationsEto)} event for tenant: {tenant.Id} {tenant.Name}.");
            await DistributedEventBus.PublishAsync(new ApplyDatabaseMigrationsEto
            {
                TenantId = tenant.Id,
                DatabaseName = eventData.DatabaseName
            });
        }
    }
}
