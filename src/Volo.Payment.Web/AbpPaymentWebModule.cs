using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Payment.Localization;

namespace Volo.Payment;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpPaymentApplicationContractsModule),
    typeof(AbpAutoMapperModule)
    )]
public class AbpPaymentWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(PaymentResource),
                typeof(AbpPaymentWebModule).Assembly,
                typeof(AbpPaymentApplicationContractsModule).Assembly
            );
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPaymentWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentWebModule>("Volo.Payment.Web");
        });

        context.Services.AddAutoMapperObjectMapper<AbpPaymentWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<AbpPaymentWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
                //Configure authorization.
            });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(AbpPaymentCommonRemoteServiceConsts.ModuleName);
        });
        
        context.Services.AddAbpDynamicOptions<PaymentWebOptions, PaymentWebOptionsManager>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
