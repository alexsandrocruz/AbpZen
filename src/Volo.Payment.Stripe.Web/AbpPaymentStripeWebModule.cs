using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Payment.Stripe.Pages;

namespace Volo.Payment.Stripe;

[DependsOn(
    typeof(AbpPaymentStripeDomainSharedModule),
    typeof(AbpPaymentWebModule)
)]
public class AbpPaymentStripeWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<PaymentWebOptions>, StripePaymentWebOptionsSetup>());

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentStripeWebModule>();
        });

        var configuration = context.Services.GetConfiguration();
        Configure<StripeWebOptions>(configuration.GetSection("Payment:Stripe"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
