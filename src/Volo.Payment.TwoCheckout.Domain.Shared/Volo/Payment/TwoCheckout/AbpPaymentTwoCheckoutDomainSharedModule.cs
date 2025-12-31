using Volo.Abp.Modularity;

namespace Volo.Payment.TwoCheckout;

[DependsOn(typeof(AbpPaymentDomainSharedModule))]
public class AbpPaymentTwoCheckoutDomainSharedModule : AbpModule
{

}
