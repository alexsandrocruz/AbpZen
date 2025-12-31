using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Payment.Admin;

[DependsOn(
    typeof(AbpPaymentApplicationModule),
    typeof(AbpPaymentAdminApplicationContractsModule))]
public class AbpPaymentAdminApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpPaymentAdminApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<PaymentAdminApplicationAutoMapperProfile>(validate: true);
        });
    }
}
