using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Payment.Paypal;

namespace Volo.Payment.PayPal;

[DependsOn(
    typeof(AbpPaymentWebModule),
    typeof(AbpPaymentPayPalDomainSharedModule)
)]
public class AbpPaymentPayPalWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<PaymentWebOptions>, PayPalPaymentWebOptionsSetup>());

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentPayPalWebModule>();
        });

        var configuration = context.Services.GetConfiguration();
        Configure<PayPalWebOptions>(configuration.GetSection("Payment:PayPal"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
