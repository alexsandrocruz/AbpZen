using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web;
using Volo.Abp.LeptonTheme;
using Volo.Abp.LeptonTheme.Management;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Contact;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.Pro.Admin.Web;
using Volo.CmsKit.Pro.Menus;
using Volo.CmsKit.Pro.MultiTenancy;
using Volo.CmsKit.Pro.Public.Web;
using Volo.CmsKit.Pro.Public.Web.Middlewares;
using Volo.CmsKit.Pro.Public.Web.Pages.Public.Newsletters;
using Volo.CmsKit.Pro.Web;
using Volo.CmsKit.Public.UrlShorting;
using Volo.CmsKit.Tags;
using Volo.Saas.Host;
using Volo.CmsKit.Faqs;


#if EntityFrameworkCore
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
#elif MongoDB
using Volo.Abp.AuditLogging.MongoDB;
using Volo.Abp.BlobStoring.Database.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.FeatureManagement.MongoDB;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;
using Volo.CmsKit.MongoDB;
using Volo.Saas.MongoDB;
#endif

namespace Volo.CmsKit.Pro;

[DependsOn(
    typeof(CmsKitProWebModule),
    typeof(CmsKitProHttpApiModule),
    typeof(CmsKitProApplicationModule),
#if EntityFrameworkCore
    typeof(CmsKitProEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpIdentityProEntityFrameworkCoreModule),
    typeof(SaasEntityFrameworkCoreModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
#elif MongoDB
    typeof(CmsKitProMongoDbModule),
    typeof(AbpAuditLoggingMongoDbModule),
    typeof(AbpMongoDbModule),
    typeof(AbpSettingManagementMongoDbModule),
    typeof(AbpPermissionManagementMongoDbModule),
    typeof(AbpIdentityProMongoDbModule),
    typeof(SaasMongoDbModule),
    typeof(BlobStoringDatabaseMongoDbModule),
    typeof(AbpFeatureManagementMongoDbModule),
#endif
    typeof(AbpAutofacModule),
    typeof(AbpAccountPublicHttpApiModule),
    typeof(AbpAccountPublicWebModule),
    typeof(AbpAccountPublicApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(LeptonThemeManagementHttpApiModule),
    typeof(LeptonThemeManagementWebModule),
    typeof(LeptonThemeManagementApplicationModule),
    typeof(LeptonThemeManagementDomainModule),
    typeof(SaasHostHttpApiModule),
    typeof(SaasHostWebModule),
    typeof(SaasHostApplicationModule),
    typeof(AbpAspNetCoreMvcUiLeptonThemeModule),
    typeof(AbpFeatureManagementApplicationModule)
)]
public class ProWebUnifiedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FeatureConfigurer.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif

#if EntityFrameworkCore
        Configure<AbpDbContextOptions>(options => { options.UseSqlServer(); });
#endif

        Configure<CmsKitTagOptions>(options =>
        {
            options.EntityTypes.Add(new TagEntityTypeDefiniton("Posts"));

            options.EntityTypes.Add(new TagEntityTypeDefiniton("Products"));
        });

        Configure<CmsKitPageFeedbackOptions>(options =>
        {
            options.EntityTypes.Add(new PageFeedbackEntityTypeDefinition("Page"));
            options.EntityTypes.Add(new PageFeedbackEntityTypeDefinition("Post"));
        });

        Configure<CmsKitPollingOptions>(options =>
        {
            options.AddWidget("poll-right");
            options.AddWidget("poll-left");
        });

        Configure<CmsKitContactConfigOptions>(options =>
        {
            options.AddContact("Sales", "info@sales.com");
            options.AddContact("Training", "info@training.com");
        });

        Configure<CmsKitCommentOptions>(options =>
        {
            options.EntityTypes.Add(new CommentEntityTypeDefinition("quote"));
        });

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<CmsKitProDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        string.Format("..{0}..{0}src{0}Volo.CmsKit.Pro.Domain.Shared",
                            Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsKitProDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        string.Format("..{0}..{0}src{0}Volo.CmsKit.Pro.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsKitProApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        string.Format("..{0}..{0}src{0}Volo.CmsKit.Pro.Application.Contracts",
                            Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsKitProApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        string.Format("..{0}..{0}src{0}Volo.CmsKit.Pro.Application", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsKitProWebModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        string.Format("..{0}..{0}src{0}Volo.CmsKit.Pro.Web", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsKitProAdminWebModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        string.Format("..{0}..{0}src{0}Volo.CmsKit.Pro.Admin.Web", Path.DirectorySeparatorChar)));

                options.FileSets.ReplaceEmbeddedByPhysical<CmsKitProPublicWebModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Volo.CmsKit.Pro.Public.Web", Path.DirectorySeparatorChar))
                    );
                
                options.FileSets.ReplaceEmbeddedByPhysical<CmsKitProCommonWebModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Volo.CmsKit.Pro.Common.Web", Path.DirectorySeparatorChar))
                    );
            });
        }

        context.Services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pro API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CmsKitProMenuContributor());
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português (Brasil)"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "Chinese"));
        });

        Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = MultiTenancyConsts.IsEnabled; });

        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });

        Configure<NewsletterOptions>(options =>
        {
            List<string> additionalPreferences = new List<string>();
            additionalPreferences.Add("ExamplePreference2");
            additionalPreferences.Add("ExamplePreference2");
            additionalPreferences.Add("ExamplePreference1");
            additionalPreferences.Add("ExamplePreference3");
            additionalPreferences.Add("ExamplePreference5");

            List<string> additionalPreferences2 = new List<string>();

            // options.WidgetViewPath = "./2Default.cshtml";

            options.AddPreference("ExamplePreference1", new NewsletterPreferenceDefinition(new LocalizableString(typeof(CmsKitResource), "ExamplePreference1"), new LocalizableString(typeof(CmsKitResource), "ExampleDefinition1"), new LocalizableString(typeof(CmsKitResource), "I agree to the Terms & Conditions and <a href=\"https://abp.io/privacy\">Privacy Policy</a>."), additionalPreferences));
            options.AddPreference("ExamplePreference2", new NewsletterPreferenceDefinition(new LocalizableString(typeof(CmsKitResource), "ExamplePreference2"), new LocalizableString(typeof(CmsKitResource), "ExampleDefinition1"), new LocalizableString(typeof(CmsKitResource), "I agree to the Terms & Conditions and <a href=\"https://abp.io/privacy\">Privacy Policy</a>."), additionalPreferences));
            options.AddPreference("ExamplePreference3", new NewsletterPreferenceDefinition(new LocalizableString(typeof(CmsKitResource), "ExamplePreference3"), definition: new LocalizableString(typeof(CmsKitResource), "ExampleDefinition2"), null, additionalPreferences2));
        });

        Configure<FaqOptions>(options =>
        {
            options.SetGroups(new[] { "General", "Community", "Support" });
        });

        Configure<NewsletterPreferencesManagementOptions>(o =>
        {
            o.Source = "MngPage";
            o.PrivacyPolicyConfirmation = new LocalizableString(typeof(CmsKitResource), "Test <a href=\"https://abp.io/privacy\">Privacy Policy</a>");
        });

        //TODO - Remove after coding the UI
        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.TokenCookie.Expiration = TimeSpan.FromDays(365);
            options.AutoValidateIgnoredHttpMethods.Add("POST");
            options.AutoValidateIgnoredHttpMethods.Add("PUT");

        });
        
        // Configure<UrlShortingOptions>(options =>
        // {
        //     options.OnConflict = urlContext =>
        //     {
        //         return Task.FromResult<ShortenedUrlDto>(null);
        //     };
        // });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseMiddleware<UrlShortingMiddleware>();

        if (context.GetEnvironment().IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseErrorPage();
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseAuthentication();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseAbpRequestLocalization();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pro API"); });

        app.UseAuditing();

        app.UseConfiguredEndpoints();

        SeedData(context);
    }

    private void SeedData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            }
        });
    }
}
