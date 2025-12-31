using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace Volo.Saas.Host;

[DependsOn(
    typeof(SaasDomainModule),
    typeof(SaasHostApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpUsersAbstractionModule)
    )]
public class SaasHostApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SaasHostApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<SaasHostApplicationAutoMapperProfile>(validate: true);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
