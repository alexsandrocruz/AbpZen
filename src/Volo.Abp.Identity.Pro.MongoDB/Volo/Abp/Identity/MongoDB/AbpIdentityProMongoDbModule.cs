using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.MongoDB;

[DependsOn(
    typeof(AbpIdentityProDomainModule),
    typeof(AbpIdentityMongoDbModule)
    )]
public class AbpIdentityProMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<IdentityProMongoDbContext>(options =>
        {
            options.ReplaceDbContext<IAbpIdentityMongoDbContext>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
