using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Payment.Iyzico;

[DependsOn(
    typeof(AbpPaymentDomainModule),
    typeof(AbpPaymentIyzicoDomainSharedModule)
    )]
public class AbpPaymentIyzicoDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PaymentOptions>(options =>
        {
            options.Gateways.Add(
                new PaymentGatewayConfiguration(
                    IyzicoConsts.GatewayName,
                    new FixedLocalizableString("Iyzico"),
                    isSubscriptionSupported: false,
                    typeof(IyzicoPaymentGateway)
                )
            );
        });

        var configuration = context.Services.GetConfiguration();
        Configure<IyzicoOptions>(configuration.GetSection("Payment:Iyzico"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
