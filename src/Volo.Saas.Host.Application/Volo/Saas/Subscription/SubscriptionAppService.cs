using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Payment.Requests;
using Volo.Saas.Editions;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Subscription;
using Volo.Saas.Tenants;

namespace Volo.Saas.Host.Application.Volo.Saas.Subscription;

public class SubscriptionAppService : SaasHostAppServiceBase, ISubscriptionAppService
{
    protected IPaymentRequestAppService PaymentRequestAppService { get; }
    protected ITenantRepository TenantRepository { get; }
    protected EditionManager EditionManager { get; }

    public SubscriptionAppService(
        IPaymentRequestAppService paymentRequestAppService,
        ITenantRepository tenantRepository,
        EditionManager editionManager)
    {
        PaymentRequestAppService = paymentRequestAppService;
        TenantRepository = tenantRepository;
        EditionManager = editionManager;
    }

    public virtual async Task<PaymentRequestWithDetailsDto> CreateSubscriptionAsync(Guid editionId, Guid tenantId)
    {
        var edition = await EditionManager.GetEditionForSubscriptionAsync(editionId);

        var paymentRequest = await PaymentRequestAppService.CreateAsync(new PaymentRequestCreateDto
        {
            Products = new List<PaymentRequestProductCreateDto>
                {
                    new PaymentRequestProductCreateDto
                    {
                        PlanId = edition.PlanId,
                        Name = edition.DisplayName,
                        Code = $"{tenantId}_{edition.PlanId}",
                        Count = 1,
                        PaymentType = PaymentType.Subscription,
                    }
                },
            ExtraProperties =
                {
                    { EditionConsts.EditionIdParameterName, editionId },
                    { TenantConsts.TenantIdParameterName, tenantId },
                }
        });

        var tenant = await TenantRepository.GetAsync(tenantId);

        tenant.EditionEndDateUtc = DateTime.UtcNow;

        await TenantRepository.UpdateAsync(tenant);

        return paymentRequest;
    }
}
