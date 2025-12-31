using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Payment.Iyzico;

[DependsOn(
    typeof(AbpPaymentWebModule),
    typeof(AbpPaymentIyzicoDomainSharedModule)
    )]
public class AbpPaymentIyzicoWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<PaymentWebOptions>, IyzicoPaymentWebOptionsSetup>());

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentIyzicoWebModule>();
        });

        var configuration = context.Services.GetConfiguration();
        Configure<IyzicoWebOptions>(configuration.GetSection("Payment:Iyzico"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
