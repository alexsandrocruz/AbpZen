using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public class ChangePasswordModel : AccountPageModel
{
    public static string ChangePasswordScheme = "Abp.ChangePassword";

    [BindProperty]
    public UserInfoModel UserInfo { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool RememberMe { get; set; }

    public bool HideOldPasswordInput { get; set; }

    [Required]
    [BindProperty]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string CurrentPassword { get; set; }

    [Required]
    [BindProperty]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string NewPassword { get; set; }

    [Required]
    [BindProperty]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DataType(DataType.Password)]
    [DisableAuditing]
    [Compare("NewPassword")]
    public string NewPasswordConfirm { get; set; }

    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }

    public ChangePasswordModel(ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        CurrentPrincipalAccessor = currentPrincipalAccessor;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        UserInfo = await RetrieveChangePasswordUser();

        if (UserInfo == null || UserInfo.TenantId != CurrentTenant.Id)
        {
            await HttpContext.SignOutAsync(ChangePasswordModel.ChangePasswordScheme);
            return RedirectToPage("./Login", new
            {
                ReturnUrl = ReturnUrl,
                ReturnUrlHash = ReturnUrlHash
            });
        }

        var user = await UserManager.GetByIdAsync(UserInfo.Id);
        HideOldPasswordInput = user.PasswordHash == null;

        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        if (CurrentPassword == NewPassword)
        {
            Alerts.Warning(L["NewPasswordSameAsOld"]);
            return Page();
        }

        var userInfo = await RetrieveChangePasswordUser();
        if (userInfo == null || userInfo.TenantId != CurrentTenant.Id)
        {
            await HttpContext.SignOutAsync(ChangePasswordModel.ChangePasswordScheme);
            return RedirectToPage("./Login", new
            {
                ReturnUrl = ReturnUrl,
                ReturnUrlHash = ReturnUrlHash
            });
        }

        using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
        {
            try
            {
                await IdentityOptions.SetAsync();
                var user = await UserManager.GetByIdAsync(userInfo.Id);
                HideOldPasswordInput = user.PasswordHash == null;

                if (user.PasswordHash == null)
                {
                    (await UserManager.AddPasswordAsync(user, NewPassword)).CheckErrors();
                }
                else
                {
                    (await UserManager.ChangePasswordAsync(user, CurrentPassword, NewPassword)).CheckErrors();
                }

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogActionConsts.ChangePassword
                });

                user.SetShouldChangePasswordOnNextLogin(false);
                (await UserManager.UpdateAsync(user)).CheckErrors();

                await HttpContext.SignOutAsync(ChangePasswordScheme);

                var result = await SignInManager.CallSignInOrTwoFactorAsync(user, RememberMe);
                if (result.Succeeded)
                {
                    await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                    {
                        Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                        Action = IdentitySecurityLogActionConsts.LoginSucceeded,
                        UserName = user.UserName
                    });

                    // Clear the dynamic claims cache.
                    await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

                    var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
                    if (loginInfo != null)
                    {
                        var externalUser = await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
                        if (externalUser != null)
                        {
                            if (!await AccountExternalLoginAppService.HasPasswordVerifiedAsync(externalUser.Id, loginInfo.LoginProvider, loginInfo.ProviderKey))
                            {
                                using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(externalUser)))
                                {
                                    await AccountExternalLoginAppService.SetPasswordVerifiedAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
                                }
                            }
                        }
                    }

                    await uow.CompleteAsync();
                    return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
                }

                await uow.CompleteAsync();

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./SendSecurityCode", new {returnUrl = ReturnUrl, returnUrlHash = ReturnUrlHash});
                }

                if (result.IsLockedOut)
                {
                    return RedirectToPage("./LockedOut", new {returnUrl = ReturnUrl, returnUrlHash = ReturnUrlHash});
                }

                return RedirectToPage("./Login", new {ReturnUrl = ReturnUrl, ReturnUrlHash = ReturnUrlHash});
            }
            catch (Exception e)
            {
                await uow.RollbackAsync();
                Alerts.Warning(GetLocalizeExceptionMessage(e));
                return Page();
            }
        }
    }

    protected virtual async Task<UserInfoModel> RetrieveChangePasswordUser()
    {
        var result = await HttpContext.AuthenticateAsync(ChangePasswordModel.ChangePasswordScheme);
        if (result?.Principal != null)
        {
            var userId = result.Principal.FindUserId();
            if (userId == null)
            {
                return null;
            }

            var tenantId = result.Principal.FindTenantId();

            using (CurrentTenant.Change(tenantId))
            {
                var user = await UserManager.FindByIdAsync(userId.Value.ToString());
                if (user == null)
                {
                    return null;
                }

                return new UserInfoModel
                {
                    Id = user.Id,
                    TenantId = user.TenantId,
                };
            }
        }

        return null;
    }

    public class UserInfoModel
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }
    }
}
