using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Payment.Payu;

[DependsOn(
    typeof(AbpPaymentDomainModule),
    typeof(AbpPaymentPayuDomainSharedModule)
    )]
public class AbpPaymentPayuDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PaymentOptions>(options =>
        {
            options.Gateways.Add(
                new PaymentGatewayConfiguration(
                    PayuConsts.GatewayName,
                    new FixedLocalizableString("Payu"),
                    isSubscriptionSupported: false,
                    typeof(PayuPaymentGateway)
                )
            );
        });

        var configuration = context.Services.GetConfiguration();
        Configure<PayuOptions>(configuration.GetSection("Payment:Payu"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
