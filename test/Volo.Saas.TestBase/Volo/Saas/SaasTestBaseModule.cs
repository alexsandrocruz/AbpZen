using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Saas;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAuthorizationModule),
    typeof(SaasDomainModule)
    )]
public class SaasTestBaseModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("Volo/Saas/appsettings.json", false);
        builder.AddJsonFile("Volo/Saas/appsettings.secrets.json", true);
        context.Services.ReplaceConfiguration(builder.Build());

        context.Services.AddAlwaysAllowAuthorization();
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            scope.ServiceProvider
                .GetRequiredService<SaasTestDataBuilder>()
                .Build();
        }
    }
}
