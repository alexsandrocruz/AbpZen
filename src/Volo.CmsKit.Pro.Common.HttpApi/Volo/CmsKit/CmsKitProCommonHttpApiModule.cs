using Volo.Abp;
using Volo.Abp.Modularity;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitCommonHttpApiModule),
    typeof(CmsKitProCommonApplicationContractsModule)
    )]
public class CmsKitProCommonHttpApiModule : AbpModule
{
    
}
