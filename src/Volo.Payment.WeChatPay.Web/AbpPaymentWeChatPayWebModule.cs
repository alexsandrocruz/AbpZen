using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Payment.WeChatPay;

[DependsOn(
    typeof(AbpPaymentWebModule),
    typeof(AbpPaymentWeChatPayDomainSharedModule)
)]
public class AbpPaymentAlipayWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<PaymentWebOptions>, WeChatPayPaymentWebOptionsSetup>());

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentAlipayWebModule>();
        });

        var configuration = context.Services.GetConfiguration();
        Configure<WeChatPayWebOptions>(configuration.GetSection("Payment:WeChatPayWeb"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}