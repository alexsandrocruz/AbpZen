using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictAuthorizeResponse
{
    public async static Task<IActionResult> GenerateAuthorizeResponseAsync(HttpContext httpContext, IdentityUser user, params Claim[] additionalClaims)
    {
        var request = httpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException(httpContext.RequestServices.GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>()["TheOpenIDConnectRequestCannotBeRetrieved"]);

        var openIddictStringLocalizer = httpContext.RequestServices.GetRequiredService<IStringLocalizer<OpenIddictAuthorizeResponse>>();
        var userManager = httpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
        var signInManager = httpContext.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
        var applicationManager = httpContext.RequestServices.GetRequiredService<IOpenIddictApplicationManager>();
        var authorizationManager = httpContext.RequestServices.GetRequiredService<IOpenIddictAuthorizationManager>();
        var scopeManager = httpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>();
        // Retrieve the application details from the database.
        var application = await applicationManager.FindByClientIdAsync(request.ClientId) ??
            throw new InvalidOperationException(openIddictStringLocalizer["DetailsConcerningTheCallingClientApplicationCannotBeFound"]);

        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = await authorizationManager.FindAsync(
            subject: await userManager.GetUserIdAsync(user),
            client: await applicationManager.GetIdAsync(application),
            status: OpenIddictConstants.Statuses.Valid,
            type: OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes: request.GetScopes()).ToListAsync();

        var principal = await signInManager.CreateUserPrincipalAsync(user);

        var identity = principal.Identities.First();
        identity.AddClaims(additionalClaims);

        // Note: in this sample, the granted scopes match the requested scope
        // but you may want to allow the user to uncheck specific scopes.
        // For that, simply restrict the list of scopes before calling SetScopes.
        principal.SetScopes(request.GetScopes());
        principal.SetResources(await scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

        // Automatically create a permanent authorization to avoid requiring explicit consent
        // for future authorization or token requests containing the same scopes.
        var authorization = authorizations.LastOrDefault();
        if (authorization == null)
        {
            authorization = await authorizationManager.CreateAsync(
                principal: principal,
                subject: await userManager.GetUserIdAsync(user),
                client: await applicationManager.GetIdAsync(application),
                type: OpenIddictConstants.AuthorizationTypes.Permanent,
                scopes: principal.GetScopes());
        }

        principal.SetAuthorizationId(await authorizationManager.GetIdAsync(authorization));

        var claimDestinationsManager = httpContext.RequestServices.GetRequiredService<AbpOpenIddictClaimsPrincipalManager>();
        await claimDestinationsManager.HandleAsync(request, principal);

        var sessionId = httpContext.RequestServices.GetRequiredService<ICurrentUser>().FindSessionId();
        if (sessionId != null)
        {
            var identitySessionManager = httpContext.RequestServices.GetRequiredService<IdentitySessionManager>();
            await identitySessionManager.RevokeAsync(sessionId);
        }

        return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, principal);
    }
}
