using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Chat.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.VirtualFileSystem;
using Volo.Chat.Authorization;
using Volo.Chat.Web.Bundling;
using Volo.Chat.Web.Settings;
using Volo.Abp.Localization;

namespace Volo.Chat.Web;

[DependsOn(
    typeof(ChatApplicationContractsModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpSettingManagementWebModule)
    )]
public class ChatWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(ChatResource), typeof(ChatWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ChatWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ChatWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<ChatWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ChatWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Chat/Index", ChatPermissions.Messaging);
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new ChatToolbarContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                    .Get(StandardBundles.Scripts.Global)
                    .AddContributors(typeof(ChatGlobalScriptContributor));
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(ChatRemoteServiceConsts.ModuleName);
        });
        
        Configure<SettingManagementPageOptions>(options =>
        {
            options.Contributors.Add(new ChatSettingManagementPageContributor());
        });
        
        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(Volo.Abp.SettingManagement.Web.Pages.SettingManagement.IndexModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/client-proxies/chat-proxy.js");
                    });
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ChatResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
