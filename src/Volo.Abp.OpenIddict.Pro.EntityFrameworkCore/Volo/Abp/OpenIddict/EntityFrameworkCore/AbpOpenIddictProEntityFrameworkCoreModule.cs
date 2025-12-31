using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore;

[DependsOn(
    typeof(AbpOpenIddictProDomainModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule)
)]
public class AbpOpenIddictProEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<OpenIddictProDbContext>(options =>
        {
            options.ReplaceDbContext<IOpenIddictDbContext, IOpenIddictProDbContext>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
