using Volo.Abp;
using Volo.Abp.Modularity;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitHttpApiModule),
    typeof(CmsKitProApplicationContractsModule),
    typeof(CmsKitProPublicHttpApiModule),
    typeof(CmsKitProAdminHttpApiModule)
    )]
public class CmsKitProHttpApiModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
