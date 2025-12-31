using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Payment.WeChatPay;

[DependsOn(
    typeof(AbpPaymentDomainModule),
    typeof(AbpPaymentWeChatPayDomainSharedModule)
)]
public class AbpPaymentWeChatPayDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PaymentOptions>(options =>
        {
            options.Gateways.Add(
                new PaymentGatewayConfiguration(
                    WeChatPayConsts.GatewayName,
                    new FixedLocalizableString("WeChatPay"),
                    isSubscriptionSupported: false,
                    typeof(WeChatPaymentGateway)
                )
            );
        });

        var configuration = context.Services.GetConfiguration();
        Configure<WeChatPayOptions>(configuration.GetSection("Payment:WeChatPay"));

        context.Services.AddHttpClient(IWeChatPayService.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(configuration["Payment:WeChatPay:BaseUrl"]);
        }).AddHttpMessageHandler<AbpWeChatPayHttpHandler>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}