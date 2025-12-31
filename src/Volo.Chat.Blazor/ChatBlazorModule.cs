using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor;
using Volo.Chat.Blazor.Settings;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.Chat.Localization;

namespace Volo.Chat.Blazor;

[DependsOn(
    typeof(ChatApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpSettingManagementBlazorModule)
    )]
public class ChatBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ChatBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<ChatBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(ChatBlazorModule).Assembly);
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new ChatToolbarContributor());
        });
        
        Configure<SettingManagementComponentOptions>(options =>
        {
            options.Contributors.Add(new ChatSettingManagementComponentContributor());
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ChatResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
