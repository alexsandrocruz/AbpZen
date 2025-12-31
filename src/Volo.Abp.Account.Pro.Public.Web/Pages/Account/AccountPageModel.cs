using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeviceDetectorNET;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Owl.reCAPTCHA;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Settings;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public abstract class AccountPageModel : AbpPageModel
{
    public IAccountAppService AccountAppService => LazyServiceProvider.LazyGetRequiredService<IAccountAppService>();
    public IAccountExternalLoginAppService AccountExternalLoginAppService => LazyServiceProvider.LazyGetRequiredService<IAccountExternalLoginAppService>();
    public AbpSignInManager SignInManager => LazyServiceProvider.LazyGetRequiredService<AbpSignInManager>();
    public IdentitySessionManager IdentitySessionManager => LazyServiceProvider.LazyGetRequiredService<IdentitySessionManager>();
    public ICurrentClient CurrentClient => LazyServiceProvider.LazyGetRequiredService<ICurrentClient>();
    public IWebClientInfoProvider WebClientInfoProvider => LazyServiceProvider.LazyGetRequiredService<IWebClientInfoProvider>();
    public IdentityUserManager UserManager => LazyServiceProvider.LazyGetRequiredService<IdentityUserManager>();
    public IdentitySecurityLogManager IdentitySecurityLogManager => LazyServiceProvider.LazyGetRequiredService<IdentitySecurityLogManager>();
    public IIdentityLinkUserAppService IdentityLinkUserAppService => LazyServiceProvider.LazyGetRequiredService<IIdentityLinkUserAppService>();
    public IOptions<IdentityOptions> IdentityOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<IdentityOptions>>();
    public IOptionsSnapshot<reCAPTCHAOptions> ReCaptchaOptions => LazyServiceProvider.LazyGetRequiredService<IOptionsSnapshot<reCAPTCHAOptions>>();
    public IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter => LazyServiceProvider.LazyGetRequiredService<IExceptionToErrorInfoConverter>();
    public IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache => LazyServiceProvider.LazyGetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();

    protected AccountPageModel()
    {
        ObjectMapperContext = typeof(AbpAccountPublicWebModule);
        LocalizationResourceType = typeof(AccountResource);
    }

    protected virtual void CheckCurrentTenant(Guid? tenantId)
    {
        if (CurrentTenant.Id != tenantId)
        {
            throw new ApplicationException(
                $"Current tenant is different than given tenant. CurrentTenant.Id: {CurrentTenant.Id}, given tenantId: {tenantId}"
            );
        }
    }

    protected virtual void CheckIdentityErrors(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new AbpIdentityResultException(identityResult);
        }
    }

    protected virtual string GetLocalizeExceptionMessage(Exception exception)
    {
        if (exception is ILocalizeErrorMessage || exception is IHasErrorCode)
        {
            return ExceptionToErrorInfoConverter.Convert(exception, false).Message;
        }

        return exception.Message;
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

    protected virtual async Task StoreChangePasswordUser(IdentityUser user)
    {
        var identity = new ClaimsIdentity(ChangePasswordModel.ChangePasswordScheme);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, user.Id.ToString()));
        if (user.TenantId.HasValue)
        {
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
        }
        await HttpContext.SignInAsync(ChangePasswordModel.ChangePasswordScheme, new ClaimsPrincipal(identity));
    }

    protected virtual async Task StoreLockedUser(IdentityUser user)
    {
        var identity = new ClaimsIdentity(LockedOut.LockedUserScheme);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, user.Id.ToString()));
        if (user.TenantId.HasValue)
        {
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
        }
        await HttpContext.SignInAsync(LockedOut.LockedUserScheme, new ClaimsPrincipal(identity));
    }

    protected virtual async Task<IActionResult> CheckLocalLoginAsync()
    {
        var enableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);
        if (!enableLocalLogin)
        {
            Alerts.Warning(L["LocalLoginIsNotEnabled"]);
            return Page();
        }

        return null;
    }

    protected virtual async Task<bool> IsValidReturnUrlAsync([NotNull] string returnUrl)
    {
        return Url.IsLocalUrl(returnUrl) ||
               returnUrl.StartsWith(UriHelper.BuildAbsolute(Request.Scheme, Request.Host, Request.PathBase).RemovePostFix("/")) ||
               await AppUrlProvider.IsRedirectAllowedUrlAsync(returnUrl);
    }
}
