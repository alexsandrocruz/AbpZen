using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation.Urls;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Newsletters;

namespace Volo.CmsKit.Pro;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAuthorizationModule),
    typeof(CmsKitProDomainModule)
    )]
public class CmsKitProTestBaseModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FeatureConfigurer.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = "https://localhost:44341/";
        });

        Configure<NewsletterOptions>(options =>
        {
            List<string> additionalPreferences = new List<string>();
            additionalPreferences.Add("Blog");
            additionalPreferences.Add("Community");
            additionalPreferences.Add("preference3");
            additionalPreferences.Add("preference2");
            additionalPreferences.Add("preference3");

            List<string> additionalPreferences2 = new List<string>();

            options.AddPreference("Community",
                new NewsletterPreferenceDefinition(
                    new LocalizableString(typeof(CmsKitResource), "Community"),
                    new LocalizableString(typeof(CmsKitResource), "CommunityDefinition"),
                    new LocalizableString(typeof(CmsKitResource), "privacy-policy-url"),
                    additionalPreferences));

            options.AddPreference("preference2",
                new NewsletterPreferenceDefinition(
                    new LocalizableString(typeof(CmsKitResource), "preference2"),
                    new LocalizableString(typeof(CmsKitResource), "definition2"),
                    new LocalizableString(typeof(CmsKitResource), "privacy-policy-url")));

            options.AddPreference("preference3",
                new NewsletterPreferenceDefinition(
                    new LocalizableString(typeof(CmsKitResource), "preference3"),
                    new LocalizableString(typeof(CmsKitResource), "definition3"),
                    additionalPreferences: additionalPreferences));

            options.AddPreference("blog",
                new NewsletterPreferenceDefinition(
                    new LocalizableString(typeof(CmsKitResource), "blog"),
                    new LocalizableString(typeof(CmsKitResource), "blog"),
                    additionalPreferences: additionalPreferences2));
        });
        
        Configure<FaqOptions>(options =>
        {
            options.SetGroups(new[] { "Group", "Group1" });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
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
