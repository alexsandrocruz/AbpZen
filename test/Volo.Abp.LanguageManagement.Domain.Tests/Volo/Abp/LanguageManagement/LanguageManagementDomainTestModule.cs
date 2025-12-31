using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Modularity;

namespace Volo.Abp.LanguageManagement;

[DependsOn(
    typeof(LanguageManagementEntityFrameworkCoreTestModule)
    )]
public class LanguageManagementDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpExternalLocalizationOptions>(options =>
        {
            options.SaveToExternalStore = false;
        });
    }
}
