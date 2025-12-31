using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Admin.Web;
using Volo.CmsKit.Admin.Web.Pages.CmsKit.BlogPosts;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Pro.Admin.Web.Menus;
using Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Newsletters.ImportToolbar;
using Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;
using Volo.CmsKit.Pro.Admin.Web.Settings;
using Volo.CmsKit.Pro.Web;
using Volo.CmsKit.Web.Contents;
using IndexModel = Volo.Abp.SettingManagement.Web.Pages.SettingManagement.IndexModel;

namespace Volo.CmsKit.Pro.Admin.Web;

[DependsOn(
    typeof(CmsKitAdminWebModule),
    typeof(CmsKitProCommonWebModule),
    typeof(CmsKitProAdminApplicationContractsModule),
    typeof(AbpSettingManagementWebModule)
    )]
public class CmsKitProAdminWebModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(CmsKitResource), typeof(CmsKitProAdminWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsKitProAdminWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CmsKitProAdminMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProAdminWebModule>();
        });

        Configure<SettingManagementPageOptions>(options =>
        {
            options.Contributors.Add(new CmsKitProSettingManagementPageContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(IndexModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/client-proxies/cms-kit-pro-admin-proxy.js");
                    });
        });

        context.Services.AddAutoMapperObjectMapper<CmsKitProAdminWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CmsKitProAdminWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizeFolder("/CmsKit/Newsletters/", CmsKitProAdminPermissions.Newsletters.Default);

            options.Conventions.AuthorizePage("/CmsKit/PageFeedbacks/Index", CmsKitProAdminPermissions.PageFeedbacks.Default);
            options.Conventions.AuthorizePage("/CmsKit/PageFeedbacks/EditModal", CmsKitProAdminPermissions.PageFeedbacks.Update);
            options.Conventions.AuthorizePage("/CmsKit/PageFeedbacks/SettingsModal", CmsKitProAdminPermissions.PageFeedbacks.Settings);
            options.Conventions.AuthorizePage("/CmsKit/PageFeedbacks/ViewModal", CmsKitProAdminPermissions.PageFeedbacks.Default);
            
            options.Conventions.AuthorizePage("/CmsKit/Polls/Index", CmsKitProAdminPermissions.Polls.Default);
            options.Conventions.AuthorizePage("/CmsKit/Polls/CreateModal", CmsKitProAdminPermissions.Polls.Create);
            options.Conventions.AuthorizePage("/CmsKit/Polls/EditModal", CmsKitProAdminPermissions.Polls.Update);
            options.Conventions.AuthorizePage("/CmsKit/Polls/ResultModal", CmsKitProAdminPermissions.Polls.Default);

            options.Conventions.AuthorizeFolder("/CmsKit/UrlShorting/", CmsKitProAdminPermissions.UrlShorting.Default);
            options.Conventions.AuthorizePage("/CmsKit/UrlShorting/Create", CmsKitProAdminPermissions.UrlShorting.Create);
            options.Conventions.AuthorizePage("/CmsKit/UrlShorting/Edit", CmsKitProAdminPermissions.UrlShorting.Update);

            options.Conventions.AuthorizePage("/CmsKit/Faqs/Index", CmsKitProAdminPermissions.Faqs.Default);
            options.Conventions.AuthorizePage("/CmsKit/Faqs/CreateSectionModal", CmsKitProAdminPermissions.Faqs.Create);
            options.Conventions.AuthorizePage("/CmsKit/Faqs/CreateQuestionModal", CmsKitProAdminPermissions.Faqs.Create);
            options.Conventions.AuthorizePage("/CmsKit/Faqs/EditSectionModal", CmsKitProAdminPermissions.Faqs.Update);
            options.Conventions.AuthorizePage("/CmsKit/Faqs/EditQuestionModal", CmsKitProAdminPermissions.Faqs.Update);
            
            options.Conventions.AddPageRoute("/CmsKit/PageFeedbacks/Index", "/Cms/PageFeedbacks");

            options.Conventions.AddPageRoute("/CmsKit/Polls/Index", "/Cms/Polls");
            options.Conventions.AddPageRoute("/CmsKit/Polls/Create", "/Cms/Polls/Create");
            options.Conventions.AddPageRoute("/CmsKit/Polls/Edit", "/Cms/Polls/Edit/{Id}");
            options.Conventions.AddPageRoute("/CmsKit/Polls/Result", "/Cms/Polls/Result/{Id}");

            options.Conventions.AddPageRoute("/CmsKit/UrlShorting/Index", "/Cms/UrlShorting");
            options.Conventions.AddPageRoute("/CmsKit/UrlShorting/Create", "/Cms/UrlShorting/Create");
            options.Conventions.AddPageRoute("/CmsKit/UrlShorting/Edit", "/Cms/UrlShorting/Edit/{Id}");

            options.Conventions.AddPageRoute("/CmsKit/Faqs/Index", "/Cms/Faqs");
            options.Conventions.AddPageRoute("/CmsKit/Faqs/CreateSection", "/Cms/Faqs/Create/Section");
            options.Conventions.AddPageRoute("/CmsKit/Faqs/EditSection", "/Cms/Faqs/Edit/Section/{Id}");
            options.Conventions.AddPageRoute("/CmsKit/Faqs/CreateQuestion", "/Cms/Faqs/Create/Question");
            options.Conventions.AddPageRoute("/CmsKit/Faqs/EditQuestion", "/Cms/Faqs/Edit/Question/{Id}");
        });

        Configure<AbpPageToolbarOptions>(options =>
        {
            options.Configure<Pages.CmsKit.Newsletters.IndexModel>(
                toolbar =>
                {
                    toolbar.AddComponent<ImportDropdownViewComponent>(requiredPolicyName: CmsKitProAdminPermissions.Newsletters.Import);
                    toolbar.AddButton(
                        LocalizableString.Create<CmsKitResource>("ExportCSV"),
                        icon: "download",
                        id: "ExportCsv"
                    );
                }
            );

            options.Configure<Pages.CmsKit.UrlShorting.IndexModel>(
                toolbar =>
                {
                    toolbar.AddButton(
                        LocalizableString.Create<CmsKitResource>("ForwardaUrl"),
                        icon: "plus",
                        name: "NewShortenedUrlButton",
                        id: "NewShortenedUrlButton",
                        requiredPolicyName: CmsKitProAdminPermissions.UrlShorting.Create
                    );
                }
            );

            options.Configure<Pages.CmsKit.Polls.IndexModel>(
                toolbar =>
                {
                    toolbar.AddButton(
                        LocalizableString.Create<CmsKitResource>("NewPoll"),
                        icon: "plus",
                        name: "NewPollButton",
                        id: "NewPollButton",
                        requiredPolicyName: CmsKitProAdminPermissions.Polls.Default
                    );
                }
            );

            options.Configure<Pages.CmsKit.PageFeedbacks.IndexModel>(
                toolbar =>
                {
                    toolbar.AddButton(LocalizableString.Create<CmsKitResource>("MailSettings"),
                        icon: "envelope",
                        name: "SettingsButton",
                        id: "SettingsButton",
                        requiredPolicyName: CmsKitProAdminPermissions.PageFeedbacks.Settings
                    );
                }
            );

            options.Configure<Pages.CmsKit.Faqs.IndexModel>(
                toolbar =>
                {
                    toolbar.AddButton(LocalizableString.Create<CmsKitResource>("NewFaq"),
                        icon: "plus",
                        name: "NewFaqButton",
                        id: "NewFaqButton",
                        requiredPolicyName: CmsKitProAdminPermissions.Faqs.Create
                    );
                }
            );

        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(CmsKitProAdminRemoteServiceConsts.ModuleName);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CmsKitResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    CmsKitProModuleExtensionConsts.ModuleName,
                    CmsKitProModuleExtensionConsts.EntityNames.Poll,
                    createFormTypes: new[] { typeof(CreateModalModel.CreatePollViewModel) },
                    editFormTypes: new[] { typeof(EditModalModel.PollEditViewModel) }
                );
        });
    }
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
