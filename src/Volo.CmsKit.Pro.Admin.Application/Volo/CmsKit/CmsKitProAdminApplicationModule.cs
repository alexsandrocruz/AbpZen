using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitProDomainModule),
    typeof(CmsKitProAdminApplicationContractsModule),
    typeof(CmsKitProCommonApplicationModule),
    typeof(CmsKitAdminApplicationModule)
    )]
public class CmsKitProAdminApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CmsKitProAdminApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CmsKitProAdminApplicationModule>(validate: true);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
