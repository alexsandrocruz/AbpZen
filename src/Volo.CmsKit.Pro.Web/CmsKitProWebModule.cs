using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.CmsKit.Pro.Admin.Web;
using Volo.CmsKit.Pro.Public.Web;
using Volo.CmsKit.Web;

namespace Volo.CmsKit.Pro.Web;

[DependsOn(
    typeof(CmsKitWebModule),
    typeof(CmsKitProApplicationContractsModule),
    typeof(CmsKitProPublicWebModule),
    typeof(CmsKitProAdminWebModule)
    )]
public class CmsKitProWebModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
