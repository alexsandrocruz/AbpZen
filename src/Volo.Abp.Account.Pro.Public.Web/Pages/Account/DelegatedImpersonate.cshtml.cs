using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

[Authorize]
[IgnoreAntiforgeryToken]
public class DelegatedImpersonateModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    [Required]
    public Guid UserDelegationId { get; set; }

    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
    protected IdentityUserDelegationManager IdentityUserDelegationManager { get; }

    public DelegatedImpersonateModel(
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IdentityUserDelegationManager identityUserDelegationManager)
    {
        CurrentPrincipalAccessor = currentPrincipalAccessor;
        IdentityUserDelegationManager = identityUserDelegationManager;
    }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(NotFound());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        if (CurrentUser.FindImpersonatorUserId() != null)
        {
            throw new BusinessException("Volo.Account:NestedImpersonationIsNotAllowed");
        }

        var userDelegation = await IdentityUserDelegationManager.FindActiveDelegationByIdAsync(UserDelegationId);
        if (userDelegation == null || userDelegation.TargetUserId != CurrentUser.Id)
        {
            throw new BusinessException("Volo.Account:InvalidUserDelegationId");
        }
        if (userDelegation.SourceUserId == CurrentUser.Id)
        {
            throw new BusinessException("Volo.Account:YouCanNotImpersonateYourself");
        }

        var user = await UserManager.FindByIdAsync(userDelegation.SourceUserId.ToString());
        if (user == null)
        {
            throw new BusinessException("Volo.Account:Volo.Account:ThereIsNoUserWithId")
                .WithData("UserId", userDelegation.SourceUserId);
        }

        var isPersistent = (await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme))?.Properties?.IsPersistent ?? false;
        await SignInManager.SignOutAsync();

        var additionalClaims = new List<Claim>();
        if (CurrentUser.Id?.ToString() != CurrentUser.FindClaim(AbpClaimTypes.ImpersonatorUserId)?.Value)
        {
            additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorUserId, CurrentUser.Id.ToString()));
            additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorUserName, CurrentUser.UserName));
            if (CurrentTenant.IsAvailable)
            {
                additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorTenantId, CurrentTenant.GetId().ToString()));
            }
        }

        var rememberMeClaim = CurrentUser.FindClaim(AbpClaimTypes.RememberMe);
        if (rememberMeClaim != null)
        {
            additionalClaims.Add(rememberMeClaim);
        }

        await SignInManager.SignInWithClaimsAsync(user, new AuthenticationProperties
        {
            IsPersistent = isPersistent
        }, additionalClaims);

        //save security log to admin user.
        var userPrincipal = await SignInManager.CreateUserPrincipalAsync(user);
        userPrincipal.Identities.First().AddClaims(additionalClaims);
        using (CurrentPrincipalAccessor.Change(userPrincipal))
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "DelegatedImpersonate"
            });
        }

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return Redirect("~/");
    }
}
