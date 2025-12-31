using Volo.Abp.Modularity;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitHttpApiClientModule),
    typeof(CmsKitProApplicationContractsModule),
    typeof(CmsKitProAdminHttpApiClientModule),
    typeof(CmsKitProPublicHttpApiClientModule)
    )]
public class CmsKitProHttpApiClientModule : AbpModule
{

}
