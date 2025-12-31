using Volo.Abp.Modularity;

namespace Volo.Payment.Payu;

[DependsOn(typeof(AbpPaymentDomainSharedModule))]
public class AbpPaymentPayuDomainSharedModule : AbpModule
{

}
