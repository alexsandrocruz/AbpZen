using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Payment.TwoCheckout;

[DependsOn(
    typeof(AbpPaymentWebModule),
    typeof(AbpPaymentTwoCheckoutDomainSharedModule)
    )]
public class AbpPaymentTwoCheckoutWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<PaymentWebOptions>, TwoCheckoutPaymentWebOptionsSetup>());

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentTwoCheckoutWebModule>();
        });

        var configuration = context.Services.GetConfiguration();
        Configure<TwoCheckoutWebOptions>(configuration.GetSection("Payment:TwoCheckout"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
