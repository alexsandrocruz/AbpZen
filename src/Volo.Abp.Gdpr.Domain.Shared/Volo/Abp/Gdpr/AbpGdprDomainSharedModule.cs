using Volo.Abp.Gdpr.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Gdpr;

[DependsOn(
    typeof(AbpLocalizationModule), 
    typeof(AbpVirtualFileSystemModule)
)]
public class AbpGdprDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpGdprDomainSharedModule>();
        });
        
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpGdprResource>("en")
                .AddVirtualJson("Volo/Abp/Gdpr/Localization");
        });
        
        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Volo.Abp.Gdpr", typeof(AbpGdprResource));
        });
    }
}