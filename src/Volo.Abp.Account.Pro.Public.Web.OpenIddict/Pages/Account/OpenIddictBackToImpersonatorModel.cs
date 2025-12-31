using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(BackToImpersonatorModel))]
public class OpenIddictBackToImpersonatorModel : BackToImpersonatorModel
{
    protected readonly AbpAccountOpenIddictOptions Options;

    public OpenIddictBackToImpersonatorModel(
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IOptions<AbpAccountOpenIddictOptions> options)
        : base(currentPrincipalAccessor)
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
        if (authenticateResult.Succeeded)
        {
            using (CurrentPrincipalAccessor.Change(authenticateResult.Principal))
            {
                var impersonatorTenantId = CurrentPrincipalAccessor.Principal.FindImpersonatorTenantId();
                var impersonatorUserId = CurrentPrincipalAccessor.Principal.FindImpersonatorUserId();

                if (impersonatorTenantId == null && impersonatorUserId == null || impersonatorUserId == null)
                {
                    return Unauthorized();
                }

                using (CurrentTenant.Change(impersonatorTenantId))
                {
                    var user = await UserManager.GetByIdAsync(impersonatorUserId.Value);
                    try
                    {
                        return await OpenIddictAuthorizeResponse.GenerateAuthorizeResponseAsync(HttpContext, user);
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                        return Unauthorized();
                    }
                }
            }
        }

        return Unauthorized();
    }
}
