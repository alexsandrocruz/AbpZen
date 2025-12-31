using Markdig;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor;
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.Pro.Admin.Blazor.Menus;
using Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Contents;
using Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.WidgetComponents;
using Volo.CmsKit.Pro.Admin.Blazor.Settings;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Admin.Blazor;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpSettingManagementBlazorModule),
    typeof(AbpGuidsModule),
    typeof(CmsKitProAdminApplicationContractsModule)
)]
public class CmsKitProAdminBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CmsKitProAdminBlazorModule>();
        
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CmsKitProAdminBlazorModule>(validate: true); });
        
        Configure<SettingManagementComponentOptions>(options =>
        {
            options.Contributors.Add(new CmsKitProAdminSettingManagementComponentContributor());
        });
        
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CmsKitProAdminMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(CmsKitProAdminBlazorModule).Assembly);
        });
        
        Configure<CmsKitContentWidgetOptions>(options =>
        {
            options.AddWidget(null, "Poll", "CmsPollByCode", "CmsPolls", parameterWidgetType: typeof(PollsComponent));
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CmsKitResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
        
        context.Services
            .AddSingleton(_ => new MarkdownPipelineBuilder()
                .UseAutoLinks()
                .UseBootstrap()
                .UseGridTables()
                .UsePipeTables()
                .Build());
    }
}
