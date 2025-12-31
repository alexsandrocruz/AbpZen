using Volo.Abp.Modularity;

namespace Volo.Payment.Alipay;

[DependsOn(typeof(AbpPaymentDomainSharedModule))]
public class AbpPaymentAlipayDomainSharedModule : AbpModule
{
    
}