using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Saas;
using Volo.Saas.Tenants;

namespace Volo.Payment.Subscription;

public class SubscriptionUpdatedHandler : IDistributedEventHandler<SubscriptionUpdatedEto>, ITransientDependency
{
    protected ITenantRepository TenantRepository { get; }
    protected ILogger<SubscriptionCreatedHandler> Logger { get; }

    public SubscriptionUpdatedHandler(
        ITenantRepository tenantRepository,
        ILogger<SubscriptionCreatedHandler> logger)
    {
        TenantRepository = tenantRepository;
        Logger = logger;
    }


    public async Task HandleEventAsync(SubscriptionUpdatedEto eventData)
    {
        var tenantId = Guid.Parse(eventData.ExtraProperties[TenantConsts.TenantIdParameterName]?.ToString());

        var tenant = await TenantRepository.FindAsync(tenantId, includeDetails: false);

        if (tenant == null)
        {
            Logger.LogWarning("Tenant couldn't be found with PaymentRequestId: " + eventData.PaymentRequestId);
            return;
        }

        tenant.EditionEndDateUtc = eventData.PeriodEndDate;
        tenant.EditionId = Guid.Parse(eventData.ExtraProperties[EditionConsts.EditionIdParameterName]?.ToString());

        await TenantRepository.UpdateAsync(tenant);
    }
}
