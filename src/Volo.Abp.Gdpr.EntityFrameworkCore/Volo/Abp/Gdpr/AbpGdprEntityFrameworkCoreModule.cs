using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr;

[DependsOn(
    typeof(AbpGdprDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpGdprEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<GdprDbContext>(options =>
        {
            options.AddRepository<GdprRequest, EfCoreGdprRequestRepository>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}