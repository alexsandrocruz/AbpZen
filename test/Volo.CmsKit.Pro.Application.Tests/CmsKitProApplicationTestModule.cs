using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;

namespace Volo.CmsKit.Pro;

[DependsOn(
    typeof(CmsKitProApplicationModule),
    typeof(CmsKitProDomainTestModule),
    typeof(AbpTextTemplatingModule),
    typeof(AbpEmailingModule)
)]
public class CmsKitProApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
    }
}
