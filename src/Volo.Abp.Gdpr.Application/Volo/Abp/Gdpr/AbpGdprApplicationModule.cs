using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr;

[DependsOn(
    typeof(AbpGdprDomainModule),
    typeof(AbpGdprApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpCachingModule)
)]
public class AbpGdprApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureAutoMapper(context);
    }
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }

    private void ConfigureAutoMapper(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpGdprApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpGdprApplicationModuleAutoMapperProfile>();
        });
    }
}