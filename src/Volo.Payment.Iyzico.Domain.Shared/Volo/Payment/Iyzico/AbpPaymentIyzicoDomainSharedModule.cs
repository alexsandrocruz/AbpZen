using Volo.Abp.Modularity;

namespace Volo.Payment.Iyzico;

[DependsOn(typeof(AbpPaymentDomainSharedModule))]
public class AbpPaymentIyzicoDomainSharedModule : AbpModule
{

}
