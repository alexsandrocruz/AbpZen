using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AuditLogging.Blazor.Menus;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.SettingManagement.Blazor;
using Volo.Abp.Settings;
using Volo.Abp.AuditLogging.Blazor.Settings;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.Abp.AuditLogging.Localization;

namespace Volo.Abp.AuditLogging.Blazor;

[DependsOn(
    typeof(AbpAuditLoggingApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpSettingManagementBlazorModule)
    )]
public class AbpAuditLoggingBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpAuditLoggingBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpAuditLoggingBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpAuditLoggingMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpAuditLoggingBlazorModule).Assembly);
        });

        Configure<SettingManagementComponentOptions>(options =>
        {
            options.Contributors.Add(new AuditLogPageContributor());
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AuditLoggingResource>()
                .AddBaseTypes(typeof(AbpUiResource));

        });
    }
}
