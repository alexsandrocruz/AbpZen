using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(DelegatedImpersonateModel))]
public class IdentityServerDelegatedImpersonateModel : DelegatedImpersonateModel
{
    protected readonly AbpAccountIdentityServerOptions Options;
    
    public IdentityServerDelegatedImpersonateModel(
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IdentityUserDelegationManager identityUserDelegationManager, 
        IOptions<AbpAccountIdentityServerOptions> options)
        : base(currentPrincipalAccessor, identityUserDelegationManager)
    {
        Options = options.Value;
    }
    
    public async override Task<IActionResult> OnGetAsync()
    {
        if (!Request.Query.ContainsKey("access_token"))
        {
            return await base.OnGetAsync();
        }

        var authenticateResult = await HttpContext.AuthenticateAsync(Options.ImpersonationAuthenticationScheme);
        if (!authenticateResult.Succeeded)
        {
            if (authenticateResult.Failure != null)
            {   
                Logger.LogException(authenticateResult.Failure);
            }

            throw new BusinessException("Volo.Account:ImpersonateError");
        }
        
        using (CurrentPrincipalAccessor.Change(authenticateResult.Principal))
        {
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
                throw new BusinessException("Volo.Account:ThereIsNoUserWithUsernameInTheTenant")
                    .WithData("UserId", userDelegation.SourceUserId);
            }
            
            var additionalClaims = new List<Claim>();
            if (CurrentUser.Id?.ToString() != CurrentUser.FindClaim(AbpClaimTypes.ImpersonatorUserId)?.Value)
            {
                additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorUserId, CurrentUser.Id.ToString()));
                additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorUserName, CurrentUser.UserName));
                if (CurrentTenant.IsAvailable)
                {
                    additionalClaims.Add(new Claim(AbpClaimTypes.ImpersonatorTenantId, CurrentTenant.Id.ToString()));
                }
            }

            try
            {
                await IdentityServerAuthorizeResponse.GenerateAuthorizeResponseAsync(HttpContext, user, additionalClaims.ToArray());
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                throw new BusinessException("Volo.Account:ImpersonateError");
            }

            return new EmptyResult();
        }
    }
}