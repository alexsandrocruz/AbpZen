using Alipay.EasySDK.Factory;
using Alipay.EasySDK.Kernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Payment.Alipay;

[DependsOn(
    typeof(AbpPaymentDomainModule),
    typeof(AbpPaymentAlipayDomainSharedModule)
)]
public class AbpPaymentAlipayDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PaymentOptions>(options =>
        {
            options.Gateways.Add(
                new PaymentGatewayConfiguration(
                    AlipayConsts.GatewayName,
                    new FixedLocalizableString("Alipay"),
                    isSubscriptionSupported: false,
                    typeof(AlipayPaymentGateway)
                )
            );
        });

        var configuration = context.Services.GetConfiguration();
        var alipaySection = configuration.GetSection("Payment:Alipay");
        
        Configure<AlipayOptions>(alipaySection);
        Factory.SetOptions(alipaySection.Get<Config>());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}