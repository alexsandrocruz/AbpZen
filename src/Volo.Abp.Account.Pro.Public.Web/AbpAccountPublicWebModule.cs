using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Owl.reCAPTCHA;
using Owl.reCAPTCHA.v2;
using Owl.reCAPTCHA.v3;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Pro.Public.Web.Shared;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.Account.Public.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;
using Volo.Abp.Account.Public.Web.ProfileManagement;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Account.Public.Web.Security.Recaptcha;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.CropperJs;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.QRCode;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Uppy;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Zxcvbn;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Commercial;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Sms;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account.Public.Web;

[DependsOn(typeof(AbpAccountPublicApplicationContractsModule))]
[DependsOn(typeof(AbpIdentityAspNetCoreModule))]
[DependsOn(typeof(AbpIdentityProDomainModule))]
[DependsOn(typeof(AbpAspNetCoreMvcUiThemeCommercialModule))]
[DependsOn(typeof(AbpEmailingModule))]
[DependsOn(typeof(AbpSmsModule))]
[DependsOn(typeof(AbpSecurityModule))]
[DependsOn(typeof(AbpAutoMapperModule))]
[DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
[DependsOn(typeof(AbpExceptionHandlingModule))]
[DependsOn(typeof(AbpAccountPublicWebSharedModule))]
public class AbpAccountPublicWebModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AccountResource),
                typeof(AbpAccountPublicApplicationContractsModule).Assembly,
                typeof(AbpAccountPublicWebModule).Assembly
            );
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountPublicWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountPublicWebModule>();
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpAccountUserMenuContributor());
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new AccountModuleToolbarContributor());
        });

        context.Services.AddAutoMapperObjectMapper<AbpAccountPublicWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpAccountProPublicWebAutomapperProfile>(validate: true);
        });

        ConfigureProfileManagement();
        ConfigureCaptcha(context);

        context.Services.AddAuthentication().AddCookie(ConfirmUserModel.ConfirmUserScheme, options =>
        {
            options.LoginPath = new PathString("/Account/Login");
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.Events = new CookieAuthenticationEvents
            {
                OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            };
        });

        context.Services.AddAuthentication().AddCookie(ChangePasswordModel.ChangePasswordScheme, options =>
        {
            options.LoginPath = new PathString("/Account/Login");
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.Events = new CookieAuthenticationEvents
            {
                OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            };
        });
        
        context.Services.AddAuthentication().AddCookie(LockedOut.LockedUserScheme, options =>
        {
            options.LoginPath = new PathString("/Account/Login");
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.Events = new CookieAuthenticationEvents
            {
                OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            };
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.FormBodyBindingIgnoredTypes.Add(typeof(ProfilePictureInput));
        });

        context.Services.ConfigureApplicationCookie(options =>
        {
            var previousOnSignedIn = options.Events.OnSignedIn;
            options.Events.OnSignedIn = async cookieSignedInContext =>
            {
                await previousOnSignedIn(cookieSignedInContext);

                var logger = cookieSignedInContext.HttpContext.RequestServices.GetRequiredService<ILogger<AbpAccountPublicWebModule>>();
                var sessionId =  cookieSignedInContext.Principal?.FindSessionId();
                if (sessionId != null)
                {
                    var identitySessionManager = cookieSignedInContext.HttpContext.RequestServices.GetRequiredService<IdentitySessionManager>();
                    var webClientInfoProvider = cookieSignedInContext.HttpContext.RequestServices.GetRequiredService<IWebClientInfoProvider>();
                    logger.LogDebug($"Creating a new SessionId({sessionId}) during sign in.");
                    await identitySessionManager.CreateAsync(
                        sessionId,
                        IdentitySessionDevices.Web,
                        webClientInfoProvider.DeviceInfo,
                        cookieSignedInContext.Principal.FindUserId()!.Value,
                        cookieSignedInContext.Principal.FindTenantId(),
                        cookieSignedInContext.Principal.FindClientId(),
                        webClientInfoProvider.ClientIpAddress,
                        true);
                }
                else
                {
                    logger.LogError("SessionId is null. It's not possible to save the session during sign in.");
                }
            };

            var previousOnSigningOut = options.Events.OnSigningOut;
            options.Events.OnSigningOut = async cookieSigningOutContext =>
            {
                await previousOnSigningOut(cookieSigningOutContext);

                var logger = cookieSigningOutContext.HttpContext.RequestServices.GetRequiredService<ILogger<AbpAccountPublicWebModule>>();
                var currentUser = cookieSigningOutContext.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
                var sessionId = currentUser.FindSessionId();
                if (sessionId != null)
                {
                    var identitySessionManager = cookieSigningOutContext.HttpContext.RequestServices.GetRequiredService<IdentitySessionManager>();
                    logger.LogDebug($"Revoking the SessionId({sessionId}) during sign out.");
                    await identitySessionManager.RevokeAsync(sessionId);
                }
                else
                {
                    logger.LogError("SessionId is null. It's not possible to revoke the session during sign out.");
                }
            };
        });
    }

    private void ConfigureProfileManagement()
    {
        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Account/Manage");
            options.Conventions.AuthorizePage("/Account/SecurityLogs");
        });

        Configure<ProfileManagementPageOptions>(options =>
        {
            options.Contributors.Add(new AccountProfileManagementPageContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(ManageModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/client-proxies/account-proxy.js");
                        configuration.AddFiles("/Pages/Account/PasswordComplexityIndicator.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/Password/Default.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/ProfilePicture/Default.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Default.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/AuthenticatorApp/Default.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/TwoFactor/Default.js");
                        configuration.AddFiles("/Pages/Account/Manage.js");
                        configuration.AddContributors(typeof(UppyScriptContributor));
                        configuration.AddContributors(typeof(CropperJsScriptContributor));
                        configuration.AddContributors(typeof(QRCodeScriptContributor));
                        configuration.AddContributors(typeof(ZxcvbnScriptContributor));
                    });
            options.StyleBundles
                .Configure(typeof(ManageModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/ProfilePicture/Default.css");
                        configuration.AddContributors(typeof(CropperJsStyleContributor));
                    });
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(AccountProPublicRemoteServiceConsts.ModuleName);
        });
    }

    private void ConfigureCaptcha(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDynamicOptions<reCAPTCHAOptions, AbpRecaptchaOptionsManager>();

        var configuration = context.Services.GetConfiguration();

        if (!configuration["Recaptcha"].IsNullOrEmpty())
        {
            Configure<reCAPTCHAOptions>(reCAPTCHAConsts.V2, configuration.GetSection("Recaptcha"));
            Configure<reCAPTCHAOptions>(reCAPTCHAConsts.V3, configuration.GetSection("Recaptcha"));
        }

        context.Services.AddTransient<IreCAPTCHALanguageCodeProvider, CultureInforeCAPTCHALanguageCodeProvider>();
        context.Services.AddTransient<IreCAPTCHASiteVerifyV2, reCAPTCHASiteVerifyV2>();
        context.Services.AddTransient<IreCAPTCHASiteVerifyV3, reCAPTCHASiteVerifyV3>();
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    editFormTypes: new[] { typeof(AccountProfilePersonalInfoManagementGroupViewComponent.PersonalInfoModel) }
                );
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
