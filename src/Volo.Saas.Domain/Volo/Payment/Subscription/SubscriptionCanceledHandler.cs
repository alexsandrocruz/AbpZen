using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Timing;
using Volo.Saas;
using Volo.Saas.Tenants;

namespace Volo.Payment.Subscription;

public class SubscriptionCanceledHandler : IDistributedEventHandler<SubscriptionCanceledEto>, ITransientDependency
{
    protected ITenantRepository TenantRepository { get; }
    protected ILogger<SubscriptionCanceledHandler> Logger { get; }

    public SubscriptionCanceledHandler(
        ITenantRepository tenantRepository,
        ILogger<SubscriptionCanceledHandler> logger)
    {
        TenantRepository = tenantRepository;
        Logger = logger;
    }

    public async Task HandleEventAsync(SubscriptionCanceledEto eventData)
    {
        var tenantId = Guid.Parse(eventData.ExtraProperties[TenantConsts.TenantIdParameterName]?.ToString());

        var tenant = await TenantRepository.FindAsync(tenantId, includeDetails: false);

        if (tenant == null)
        {
            Logger.LogWarning("Tenant couldn't be found with PaymentRequestId: " + eventData.PaymentRequestId);
            return;
        }

        tenant.EditionEndDateUtc = eventData.PeriodEndDate;

        await TenantRepository.UpdateAsync(tenant);
    }
}
