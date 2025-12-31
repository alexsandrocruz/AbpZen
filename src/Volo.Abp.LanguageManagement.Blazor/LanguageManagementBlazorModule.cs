using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.LanguageManagement.Blazor.Menus;
using Volo.Abp.LanguageManagement.Dto;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.Abp.LanguageManagement.Localization;

namespace Volo.Abp.LanguageManagement.Blazor;

[DependsOn(
     typeof(LanguageManagementApplicationContractsModule),
     typeof(AbpAutoMapperModule),
     typeof(AbpAspNetCoreComponentsWebThemingModule)
     )]
public class LanguageManagementBlazorModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<LanguageManagementBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<LanguageManagementBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new LanguageManagementMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(LanguageManagementBlazorModule).Assembly);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<LanguageManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    LanguageManagementModuleExtensionConsts.ModuleName,
                    LanguageManagementModuleExtensionConsts.EntityNames.Language,
                    createFormTypes: new[] { typeof(CreateLanguageDto) },
                    editFormTypes: new[] { typeof(UpdateLanguageDto) }
                );
        });
    }
}
