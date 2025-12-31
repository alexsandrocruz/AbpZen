using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Payment;

[DependsOn(
    typeof(AbpPaymentDomainModule),
    typeof(AbpAutoMapperModule))]
public class AbpPaymentApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpPaymentApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<PaymentApplicationAutoMapperProfile>(validate: true);
        });
    }
}
