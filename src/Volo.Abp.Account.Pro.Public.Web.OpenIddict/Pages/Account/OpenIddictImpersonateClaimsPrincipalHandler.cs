using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictImpersonateClaimsPrincipalHandler : IAbpOpenIddictClaimsPrincipalHandler, ITransientDependency
{
    public virtual Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context)
    {
        foreach (var claim in context.Principal.Claims)
        {
            if (claim.Type == AbpClaimTypes.ImpersonatorTenantId ||
                claim.Type == AbpClaimTypes.ImpersonatorTenantName ||
                claim.Type == AbpClaimTypes.ImpersonatorUserId ||
                claim.Type == AbpClaimTypes.ImpersonatorUserName)
            {
                claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
            }
        }

        return Task.CompletedTask;
    }
}
