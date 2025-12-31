using Volo.Abp.Modularity;

namespace Volo.Payment.Paypal;

[DependsOn(typeof(AbpPaymentDomainSharedModule))]
public class AbpPaymentPayPalDomainSharedModule : AbpModule
{

}
