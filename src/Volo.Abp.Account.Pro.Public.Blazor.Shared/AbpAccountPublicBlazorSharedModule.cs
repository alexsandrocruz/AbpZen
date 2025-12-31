using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.Account.Pro.Public.Blazor.Shared;

[DependsOn(typeof(AbpAspNetCoreComponentsWebThemingModule))]
[DependsOn(typeof(AbpAccountPublicApplicationContractsModule))]
public class AbpAccountPublicBlazorSharedModule : AbpModule
{

}
