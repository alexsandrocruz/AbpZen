using Volo.Abp.Modularity;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitCommonApplicationModule),
    typeof(CmsKitProCommonApplicationContractsModule)
    )]
public class CmsKitProCommonApplicationModule : AbpModule
{
    
}