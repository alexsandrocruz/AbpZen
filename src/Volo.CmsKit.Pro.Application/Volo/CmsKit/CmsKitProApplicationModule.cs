using Volo.Abp;
using Volo.Abp.Modularity;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitApplicationModule),
    typeof(CmsKitProApplicationContractsModule),
    typeof(CmsKitProPublicApplicationModule),
    typeof(CmsKitProAdminApplicationModule)
    )]
public class CmsKitProApplicationModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
