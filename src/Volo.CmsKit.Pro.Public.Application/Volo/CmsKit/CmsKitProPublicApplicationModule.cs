using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Public;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitProDomainModule),
    typeof(CmsKitProPublicApplicationContractsModule),
    typeof(CmsKitPublicApplicationModule),
    typeof(CmsKitProCommonApplicationModule),
    typeof(AbpEmailingModule),
    typeof(AbpUiNavigationModule)
    )]
public class CmsKitProPublicApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CmsKitProPublicApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CmsKitProPublicApplicationModule>(validate: true);
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProPublicApplicationModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
