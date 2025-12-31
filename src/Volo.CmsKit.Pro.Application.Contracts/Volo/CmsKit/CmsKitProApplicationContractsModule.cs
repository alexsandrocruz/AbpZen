using Volo.Abp.Modularity;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitApplicationContractsModule),
    typeof(CmsKitProPublicApplicationContractsModule),
    typeof(CmsKitProAdminApplicationContractsModule)
    )]
public class CmsKitProApplicationContractsModule : AbpModule
{

}
