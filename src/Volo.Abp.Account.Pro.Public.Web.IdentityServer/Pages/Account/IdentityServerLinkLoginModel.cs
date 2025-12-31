using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(LinkLoginModel))]
public class IdentityServerLinkLoginModel : LinkLoginModel
{
    protected readonly AbpAccountIdentityServerOptions Options;

    public IdentityServerLinkLoginModel(ICurrentPrincipalAccessor currentPrincipalAccessor,
        IOptions<AbpAccountOptions> accountOptions,
        ITenantStore tenantStore,
        IOptions<AbpAccountIdentityServerOptions> options)
        : base(currentPrincipalAccessor, accountOptions, tenantStore)
    {
        Options = options.Value;
    }

    public async override Task<IActionResult> OnPostAsync()
    {
        var accessToken = Request.Query["access_token"].ToString();
        if (accessToken.IsNullOrEmpty())
        {
            return await base.OnPostAsync();
        }

        var authenticateResult = await HttpContext.AuthenticateAsync(Options.LinkLoginAuthenticationScheme);
        if (authenticateResult.Succeeded)
        {
            using (CurrentPrincipalAccessor.Change(authenticateResult.Principal))
            {
                using (CurrentTenant.Change(authenticateResult.Principal.FindTenantId()))
                {
                    return await base.OnPostAsync();
                }
            }
        }

        return await base.OnPostAsync();
    }

}
