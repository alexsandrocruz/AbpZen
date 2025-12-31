using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict.MongoDB;

[DependsOn(
    typeof(AbpOpenIddictProDomainModule),
    typeof(AbpOpenIddictMongoDbModule)
)]
public class AbpOpenIddictProMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<OpenIddictProMongoDbDbContext>(options =>
        {
            options.ReplaceDbContext<IOpenIddictProMongoDbDbContext>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
