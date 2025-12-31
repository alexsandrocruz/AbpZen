using Volo.Abp.Modularity;
using Volo.Abp.Caching;
using Volo.CmsKit.Public;

namespace Volo.CmsKit;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(CmsKitPublicApplicationContractsModule),
    typeof(CmsKitProCommonApplicationContractsModule)
    )]
public class CmsKitProPublicApplicationContractsModule : AbpModule
{

}
