using Localization.Resources.AbpUi;
using LeptonXDemoApp.Localization;
using Volo.Abp.Account;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Saas.Host;
using Volo.Abp.LeptonTheme;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Chat;
using Volo.CmsKit;
using Volo.FileManagement;
using Volo.Forms;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict;

namespace LeptonXDemoApp
{
    [DependsOn(
        typeof(LeptonXDemoAppApplicationContractsModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(AbpAuditLoggingHttpApiModule),
        typeof(AbpOpenIddictProHttpApiModule),
        typeof(AbpAccountAdminHttpApiModule),
        typeof(AbpAccountPublicHttpApiModule),
        typeof(LanguageManagementHttpApiModule),
        typeof(SaasHostHttpApiModule),
        typeof(LeptonThemeManagementHttpApiModule),
        typeof(TextTemplateManagementHttpApiModule)
        )]
    [DependsOn(typeof(ChatHttpApiModule))]
    [DependsOn(typeof(FileManagementHttpApiModule))]
    [DependsOn(typeof(FormsHttpApiModule))]
    [DependsOn(typeof(CmsKitProHttpApiModule))]
    [DependsOn(typeof(AbpGdprHttpApiModule))]
    public class LeptonXDemoAppHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<LeptonXDemoAppResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
