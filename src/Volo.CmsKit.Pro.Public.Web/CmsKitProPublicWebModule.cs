using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Owl.reCAPTCHA;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Pro.Public.Web.Menus;
using Volo.CmsKit.Pro.Web;
using Volo.CmsKit.Public;
using Volo.CmsKit.Public.Web;
using Volo.CmsKit.Web.Contents;

namespace Volo.CmsKit.Pro.Public.Web;

[DependsOn(
    typeof(CmsKitProPublicApplicationContractsModule),
    typeof(CmsKitPublicWebModule),
    typeof(CmsKitProCommonWebModule)
    )]
public class CmsKitProPublicWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(CmsKitResource), typeof(CmsKitProPublicWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsKitProPublicWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CmsKitProPublicMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProPublicWebModule>("Volo.CmsKit.Pro.Public.Web");
        });

        context.Services.AddAutoMapperObjectMapper<CmsKitProPublicWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CmsKitProPublicWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AddPageRoute("/Public/Newsletters/EmailPreferences", "cms/newsletter/email-preferences");
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(CmsKitProCommonRemoteServiceConsts.ModuleName);
        });

        context.Services.AddreCAPTCHAV3(x =>
        {
            x.SiteKey = configuration["CmsKit:Contact:SiteKey"];
            x.SiteSecret = configuration["CmsKit:Contact:SiteSecret"];
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
