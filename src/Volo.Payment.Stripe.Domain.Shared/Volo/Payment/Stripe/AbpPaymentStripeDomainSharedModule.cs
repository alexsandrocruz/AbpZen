using Volo.Abp.Modularity;

namespace Volo.Payment.Stripe;

[DependsOn(typeof(AbpPaymentDomainSharedModule))]
public class AbpPaymentStripeDomainSharedModule : AbpModule
{

}
