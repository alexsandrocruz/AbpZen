using Volo.Abp.Modularity;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitCommonApplicationContractsModule),
    typeof(CmsKitProDomainSharedModule)
    )]
public class CmsKitProCommonApplicationContractsModule : AbpModule
{

}
