using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Owl.reCAPTCHA;
using Volo.Abp.Account.ExternalProviders;
using Volo.Abp.Account.Public.Web.Security.Recaptcha;
using Volo.Abp.Account.Settings;
using Volo.Abp.Auditing;
using Volo.Abp.Content;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Reflection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public class RegisterModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [BindProperty]
    public PostInput Input { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool IsExternalLogin { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkUserId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkTenantId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string LinkToken { get; set; }

    public bool UseCaptcha { get; set; }

    public IEnumerable<ExternalProviderModel> ExternalProviders { get; set; }
    public IEnumerable<ExternalProviderModel> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));
    public bool EnableLocalRegister { get; set; }
    public bool EnableLocalLogin { get; set; }
    public bool IsExternalLoginOnly => EnableLocalRegister == false && ExternalProviders?.Count() == 1;
    public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

    protected readonly IAuthenticationSchemeProvider SchemeProvider;
    protected readonly AbpAccountOptions AccountOptions;
    protected readonly IAccountExternalProviderAppService AccountExternalProviderAppService;
    protected readonly ICurrentPrincipalAccessor CurrentPrincipalAccessor;
    protected readonly IHttpClientFactory HttpClientFactory;

    public RegisterModel(
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions,
        IAccountExternalProviderAppService accountExternalProviderAppService,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IHttpClientFactory httpClientFactory)
    {
        SchemeProvider = schemeProvider;
        AccountOptions = accountOptions.Value;
        AccountExternalProviderAppService = accountExternalProviderAppService;
        CurrentPrincipalAccessor = currentPrincipalAccessor;
        HttpClientFactory = httpClientFactory;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ExternalProviders = await GetExternalProviders();

        if (!await CheckSelfRegistrationAsync())
        {
            if (IsExternalLoginOnly)
            {
                return await OnPostExternalLogin(ExternalLoginScheme);
            }

            Alerts.Warning(L["SelfRegistrationDisabledMessage"]);
            return Page();
        }


        await TrySetEmailAsync();
        await SetUseCaptchaAsync();

        return Page();
    }

    [UnitOfWork] //TODO: Will be removed when we implement action filter
    public virtual async Task<IActionResult> OnPostAsync()
    {
        try
        {
            ExternalProviders = await GetExternalProviders();

            if (!await CheckSelfRegistrationAsync())
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }

            await SetUseCaptchaAsync();

            IdentityUser user;
            if (IsExternalLogin)
            {
                var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    Logger.LogWarning("External login info is not available");
                    return RedirectToPage("./Login");
                }

                if (Input.UserName.IsNullOrWhiteSpace())
                {
                    Input.UserName = await UserManager.GetUserNameFromEmailAsync(Input.EmailAddress);
                }
                user = await RegisterExternalUserAsync(externalLoginInfo, Input.UserName, Input.EmailAddress);

                // Clear the dynamic claims cache.
                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
            }
            else
            {
                user = await RegisterLocalUserAsync();
            }

            if (await SettingProvider.IsTrueAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail) && !user.EmailConfirmed ||
                await SettingProvider.IsTrueAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber) && !user.PhoneNumberConfirmed)
            {
                await StoreConfirmUser(user);

                return RedirectToPage("./ConfirmUser", new {
                    returnUrl = ReturnUrl,
                    returnUrlHash = ReturnUrlHash
                });
            }

            if (await VerifyLinkTokenAsync())
            {
                using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(user)))
                {
                    await IdentityLinkUserAppService.LinkAsync(new LinkUserInput
                    {
                        UserId = LinkUserId.Value,
                        TenantId = LinkTenantId,
                        Token = LinkToken
                    });

                    await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                    {
                        Identity = IdentitySecurityLogIdentityConsts.Identity,
                        Action = IdentityProSecurityLogActionConsts.LinkUser,
                        UserName = user.UserName,
                        ExtraProperties =
                            {
                                { IdentityProSecurityLogActionConsts.LinkTargetTenantId, LinkTenantId },
                                { IdentityProSecurityLogActionConsts.LinkTargetUserId, LinkUserId }
                            }
                    });

                    using (CurrentTenant.Change(LinkTenantId))
                    {
                        var targetUser = await UserManager.GetByIdAsync(LinkUserId.Value);
                        using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(targetUser)))
                        {
                            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                            {
                                Identity = IdentitySecurityLogIdentityConsts.Identity,
                                Action = IdentityProSecurityLogActionConsts.LinkUser,
                                UserName = targetUser.UserName,
                                ExtraProperties =
                                    {
                                        { IdentityProSecurityLogActionConsts.LinkTargetTenantId, targetUser.TenantId },
                                        { IdentityProSecurityLogActionConsts.LinkTargetUserId, targetUser.Id }
                                    }
                            });
                        }
                    }
                }
            }

            if (IsExternalLogin && await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                await StoreChangePasswordUser(user);
                return RedirectToPage("./ChangePassword", new {
                    returnUrl = ReturnUrl ?? "/",
                    returnUrlHash = ReturnUrlHash
                });
            }

            await SignInManager.SignInAsync(user, isPersistent: true);

            // Clear the dynamic claims cache.
            await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

            return Redirect(ReturnUrl ?? "/"); //TODO: How to ensure safety? IdentityServer requires it however it should be checked somehow!
        }
        catch (BusinessException e)
        {
            Alerts.Danger(GetLocalizeExceptionMessage(e));
            return Page();
        }
    }

    protected virtual async Task<IdentityUser> RegisterLocalUserAsync()
    {
        ValidateModel();

        var captchaResponse = string.Empty;
        if (UseCaptcha)
        {
            captchaResponse = HttpContext.Request.Form[RecaptchaValidatorBase.RecaptchaResponseKey];
        }
        var userDto = await AccountAppService.RegisterAsync(
            new RegisterDto
            {
                AppName = "MVC",
                EmailAddress = Input.EmailAddress,
                Password = Input.Password,
                UserName = Input.UserName,
                ReturnUrl = ReturnUrl,
                ReturnUrlHash = ReturnUrlHash,
                CaptchaResponse = captchaResponse
            }
        );

        return await UserManager.GetByIdAsync(userDto.Id);
    }

    protected virtual async Task<IdentityUser> RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string userName, string emailAddress)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress, CurrentTenant.Id);

        (await UserManager.CreateAsync(user)).CheckErrors();
        (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

        if (!user.EmailConfirmed)
        {
            await AccountAppService.SendEmailConfirmationTokenAsync(
                new SendEmailConfirmationTokenDto
                {
                    AppName = "MVC",
                    UserId = user.Id,
                    ReturnUrl = ReturnUrl,
                    ReturnUrlHash = ReturnUrlHash
                }
            );
        }

        user.Name = externalLoginInfo.Principal.FindFirstValue(AbpClaimTypes.Name) ?? externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName);
        user.Surname = externalLoginInfo.Principal.FindFirstValue(AbpClaimTypes.SurName) ?? externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname);

        var phoneNumber = externalLoginInfo.Principal.FindFirstValue(AbpClaimTypes.PhoneNumber);
        if (!phoneNumber.IsNullOrWhiteSpace())
        {
            var phoneNumberConfirmed = string.Equals(externalLoginInfo.Principal.FindFirstValue(AbpClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);
            user.SetPhoneNumber(phoneNumber, phoneNumberConfirmed);
        }

        var picture = externalLoginInfo.Principal.FindFirstValue(AbpClaimTypes.Picture);
        if (!picture.IsNullOrWhiteSpace())
        {
            var httpClient = HttpClientFactory.CreateClient();
            if (externalLoginInfo.AuthenticationTokens != null && externalLoginInfo.AuthenticationTokens.Any())
            {
                var token = externalLoginInfo.AuthenticationTokens.FirstOrDefault(x => x.Name == "access_token")?.Value;
                httpClient.SetBearerToken(token!);
            }

            try
            {
                var imageSteam = await httpClient.GetStreamAsync(picture);
                using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(user)))
                {
                    await AccountAppService.SetProfilePictureAsync(new ProfilePictureInput
                    {
                        ImageContent = new RemoteStreamContent(imageSteam), Type = ProfilePictureType.Image
                    });
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, "Could not set profile picture for user {0}", user.Id);
                //ignore
            }
        }

        var userLoginAlreadyExists = user.Logins.Any(x =>
            x.TenantId == user.TenantId &&
            x.LoginProvider == externalLoginInfo.LoginProvider &&
            x.ProviderKey == externalLoginInfo.ProviderKey);

        if (!userLoginAlreadyExists)
        {
            user.AddLogin(new UserLoginInfo(
                    externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey,
                    externalLoginInfo.ProviderDisplayName
                )
            );

            (await UserManager.UpdateAsync(user)).CheckErrors();
        }

        return user;
    }

    protected virtual async Task<bool> CheckSelfRegistrationAsync()
    {
        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);
        EnableLocalRegister = await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) &&
                              EnableLocalLogin;

        if (IsExternalLogin)
        {
            return true;
        }

        if (!EnableLocalRegister)
        {
            return false;
        }

        return true;
    }

    protected virtual async Task SetUseCaptchaAsync()
    {
        UseCaptcha = !IsExternalLogin && await SettingProvider.IsTrueAsync(AccountSettingNames.Captcha.UseCaptchaOnRegistration);
        if (UseCaptcha)
        {
            var reCaptchaVersion = await SettingProvider.GetAsync<int>(AccountSettingNames.Captcha.Version);
            await ReCaptchaOptions.SetAsync(reCaptchaVersion == 3 ? reCAPTCHAConsts.V3 : reCAPTCHAConsts.V2);
        }
    }

    protected virtual async Task StoreConfirmUser(IdentityUser user)
    {
        var identity = new ClaimsIdentity(ConfirmUserModel.ConfirmUserScheme);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, user.Id.ToString()));
        if (user.TenantId.HasValue)
        {
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
        }
        await HttpContext.SignInAsync(ConfirmUserModel.ConfirmUserScheme, new ClaimsPrincipal(identity));
    }

    protected virtual async Task<List<ExternalProviderModel>> GetExternalProviders()
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var externalProviders = await AccountExternalProviderAppService.GetAllAsync();

        var externalProviderModels = new List<ExternalProviderModel>();
        foreach (var scheme in schemes)
        {
            if (IsRemoteAuthenticationHandler(scheme, externalProviders) || scheme.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
            {
                externalProviderModels.Add(new ExternalProviderModel
                {
                    DisplayName = scheme.DisplayName,
                    AuthenticationScheme = scheme.Name,
                    Icon = AccountOptions.ExternalProviderIconMap.GetOrDefault(scheme.Name)
                });
            }
        }

        return externalProviderModels;
    }

    protected virtual bool IsRemoteAuthenticationHandler(AuthenticationScheme scheme, ExternalProviderDto externalProviders)
    {
        if (ReflectionHelper.IsAssignableToGenericType(scheme.HandlerType, typeof(RemoteAuthenticationHandler<>)))
        {
            var provider = externalProviders.Providers.FirstOrDefault(x => x.Name == scheme.Name);
            return provider == null || provider.Enabled;
        }

        return false;
    }

    [UnitOfWork]
    public virtual async Task<IActionResult> OnPostExternalLogin(string provider)
    {
        var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return await Task.FromResult(Challenge(properties, provider));
    }

    protected virtual async Task TrySetEmailAsync()
    {
        if (IsExternalLogin)
        {
            var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return;
            }

            if (!externalLoginInfo.Principal.Identities.Any())
            {
                return;
            }

            var identity = externalLoginInfo.Principal.Identities.First();
            var emailClaim = identity.FindFirst(AbpClaimTypes.Email) ?? identity.FindFirst(ClaimTypes.Email);

            if (emailClaim == null)
            {
                return;
            }

            var userName = await UserManager.GetUserNameFromEmailAsync(emailClaim.Value);
            Input = new PostInput { UserName = userName, EmailAddress = emailClaim.Value };
        }
    }

    protected virtual async Task<bool> VerifyLinkTokenAsync()
    {
        if (LinkToken.IsNullOrWhiteSpace() || LinkUserId == null)
        {
            return false;
        }

        return await IdentityLinkUserAppService.VerifyLinkTokenAsync(new VerifyLinkTokenInput
        {
            UserId = LinkUserId.Value,
            TenantId = LinkTenantId,
            Token = LinkToken
        });
    }

    public class PostInput
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }
    }

    public class ExternalProviderModel
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }

        public string Icon { get; set; }
    }
}
