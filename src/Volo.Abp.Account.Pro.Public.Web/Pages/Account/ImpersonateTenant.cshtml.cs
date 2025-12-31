using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

[Authorize]
[IgnoreAntiforgeryToken]
public class ImpersonateTenantModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    [Required]
    public Guid TenantId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string TenantUserName { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    protected AbpAccountOptions AccountOptions { get; }
    protected IPermissionChecker PermissionChecker { get; }
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }

    public ImpersonateTenantModel(
        IOptions<AbpAccountOptions> accountOptions,
        IPermissionChecker permissionChecker,
        ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        AccountOptions = accountOptions.Value;
        PermissionChecker = permissionChecker;
        CurrentPrincipalAccessor = currentPrincipalAccessor;
    }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(NotFound());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (ReturnUrl != null)
            {
                if (!Url.IsLocalUrl(ReturnUrl) &&
                    !ReturnUrl.StartsWith(UriHelper.BuildAbsolute(Request.Scheme, Request.Host, Request.PathBase).RemovePostFix("/")) &&
                    !await AppUrlProvider.IsRedirectAllowedUrlAsync(ReturnUrl))
                {
                    ReturnUrl = null;
                }
            }

            if (CurrentUser.FindImpersonatorUserId() != null)
            {
                throw new BusinessException("Volo.Account:NestedImpersonationIsNotAllowed");
            }

            if (CurrentTenant.Id != null)
            {
                throw new BusinessException("Volo.Account:ImpersonateTenantOnlyAvailableForHost");
            }

            if (AccountOptions.ImpersonationTenantPermission.IsNullOrWhiteSpace() ||
                !await PermissionChecker.IsGrantedAsync(AccountOptions.ImpersonationTenantPermission))
            {
                throw new BusinessException("Volo.Account:RequirePermissionToImpersonateTenant")
                    .WithData("PermissionName", AccountOptions.ImpersonationTenantPermission);
            }

            var currentUserId = CurrentUser.Id;
            using (CurrentTenant.Change(TenantId))
            {
                if(TenantUserName.IsNullOrWhiteSpace())
                {
                    TenantUserName = AccountOptions.TenantAdminUserName;
                }

                var adminUser = await UserManager.FindByNameAsync(TenantUserName);
                if (adminUser != null)
                {
                    var isPersistent = (await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme))?.Properties?.IsPersistent ?? false;
                    await SignInManager.SignOutAsync();

                    var additionalClaims = new List<Claim>
                    {
                        new Claim(AbpClaimTypes.ImpersonatorUserId, currentUserId.ToString()),
                        new Claim(AbpClaimTypes.ImpersonatorUserName, CurrentUser.UserName)
                    };

                    var rememberMeClaim = CurrentUser.FindClaim(AbpClaimTypes.RememberMe);
                    if (rememberMeClaim != null)
                    {
                        additionalClaims.Add(rememberMeClaim);
                    }

                    await SignInManager.SignInWithClaimsAsync(adminUser, new AuthenticationProperties
                    {
                        IsPersistent = isPersistent
                    }, additionalClaims);

                    //save security log to admin user.
                    var userPrincipal = await SignInManager.CreateUserPrincipalAsync(adminUser);
                    userPrincipal.Identities.First().AddClaims(additionalClaims);
                    using (CurrentPrincipalAccessor.Change(userPrincipal))
                    {
                        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                        {
                            Identity = IdentitySecurityLogIdentityConsts.Identity,
                            Action = "ImpersonateUser"
                        });
                    }

                    // Clear the dynamic claims cache.
                    await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(adminUser.Id, adminUser.TenantId);

                    return Redirect("~/");
                }

                throw new BusinessException("Volo.Account:ThereIsNoUserWithUserName")
                    .WithData("UserName", TenantUserName);
            }

        }
        catch(BusinessException ex)
        {
            var errorInfo = ExceptionToErrorInfoConverter.Convert(ex);
            Alerts.Danger(errorInfo.Message);
            return Page();
        }
    }
}
