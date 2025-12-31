using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Payment.Admin;

namespace Volo.Payment;

[DependsOn(
    typeof(AbpPaymentAdminApplicationModule),
    typeof(PaymentDomainTestModule)
    )]
public class PaymentApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PaymentOptions>(options =>
        {
            options.Gateways.Add(
                new PaymentGatewayConfiguration(
                    "Test",
                    new FixedLocalizableString("Test"),
                    isSubscriptionSupported: false,
                    typeof(TestPaymentGateway)
                )
            );
        });
    }
}
