using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Payment.Alipay;

[DependsOn(
    typeof(AbpPaymentWebModule),
    typeof(AbpPaymentAlipayDomainSharedModule)
)]
public class AbpPaymentAlipayWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<PaymentWebOptions>, AlipayPaymentWebOptionsSetup>());

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentAlipayWebModule>();
        });

        var configuration = context.Services.GetConfiguration();
        Configure<AlipayWebOptions>(configuration.GetSection("Payment:AlipayWeb"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}