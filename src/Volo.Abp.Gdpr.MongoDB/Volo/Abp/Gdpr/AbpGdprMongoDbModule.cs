using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Gdpr;

[DependsOn(
    typeof(AbpGdprDomainModule),
    typeof(AbpMongoDbModule)
)]
public class AbpGdprMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<GdprMongoDbContext>(options =>
        {
            options.AddRepository<GdprRequest, MongoGdprRequestRepository>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}