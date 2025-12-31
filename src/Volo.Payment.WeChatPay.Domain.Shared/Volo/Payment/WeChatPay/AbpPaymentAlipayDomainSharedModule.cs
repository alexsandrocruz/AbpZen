using Volo.Abp.Modularity;

namespace Volo.Payment.WeChatPay;

[DependsOn(typeof(AbpPaymentDomainSharedModule))]
public class AbpPaymentWeChatPayDomainSharedModule : AbpModule
{
    
}