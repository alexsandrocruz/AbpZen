using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitProAdminApplicationContractsModule),
    typeof(CmsKitAdminHttpApiModule),
    typeof(CmsKitProCommonHttpApiModule)
    )]
public class CmsKitProAdminHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsKitProAdminHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CmsKitResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
