using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Saas.Host;

namespace Volo.Saas.Tenant;

[DependsOn(
    typeof(SaasTenantApplicationModule),
    typeof(SaasHostApplicationModule),
    typeof(SaasDomainTestModule)
    )]
public class SaasTenantApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }
}
