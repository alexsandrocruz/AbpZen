using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.Account.Web.ViewModels.Verify;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Controllers;

namespace Volo.Abp.Account.Web.Controllers;

[Authorize]
[Route("connect/verify")]
[ApiExplorerSettings(IgnoreApi = true)]
public class VerifyController : AbpOpenIdDictControllerBase
{
    [HttpGet]
    public virtual async Task<IActionResult> GetAsync()
    {
        var request = await GetOpenIddictServerRequestAsync(HttpContext);

        // If the user code was not specified in the query string (e.g as part of the verification_uri_complete),
        // render a form to ask the user to enter the user code manually (non-digit chars are automatically ignored).
        if (string.IsNullOrEmpty(request.UserCode))
        {
            return View("Verify", new VerifyViewModel());
        }

        // Retrieve the claims principal associated with the user code.
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (result.Succeeded)
        {
            // Retrieve the application details from the database using the client_id stored in the principal.
            var application = await ApplicationManager.FindByClientIdAsync(result.Principal.GetClaim(OpenIddictConstants.Claims.ClientId)) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Render a form asking the user to confirm the authorization demand.
            return View("Verify", new VerifyViewModel
            {
                ApplicationName = await ApplicationManager.GetLocalizedDisplayNameAsync(application),
                Scope = string.Join(" ", result.Principal.GetScopes()),
                UserCode = request.UserCode
            });
        }

        // Redisplay the form when the user code is not valid.
        return View("Verify", new VerifyViewModel
        {
            Error = OpenIddictConstants.Errors.InvalidToken,
            ErrorDescription = "The specified user code is not valid. Please make sure you typed it correctly."
        });
    }

    [HttpPost]
    public virtual async Task<IActionResult> PostAsync()
    {
        if (await HasFormValueAsync("deny"))
        {
            // Notify OpenIddict that the authorization grant has been denied by the resource owner.
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties() {
                    // This property points to the address OpenIddict will automatically
                    // redirect the user to after rejecting the authorization demand.
                    RedirectUri = "/"
                });
        }

        // Retrieve the profile of the logged in user.
        var user = await UserManager.GetUserAsync(User) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        // Retrieve the claims principal associated with the user code.
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (result.Succeeded)
        {
            var principal = await SignInManager.CreateUserPrincipalAsync(user);

            // Note: in this sample, the granted scopes match the requested scope
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.
            principal.SetScopes(result.Principal.GetScopes());
            principal.SetResources(await ScopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

            await OpenIddictClaimsPrincipalManager.HandleAsync(await GetOpenIddictServerRequestAsync(HttpContext), principal);

            var properties = new AuthenticationProperties
            {
                // This property points to the address OpenIddict will automatically
                // redirect the user to after validating the authorization demand.
                RedirectUri = "/"
            };

            return SignIn(principal, properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Redisplay the form when the user code is not valid.
        return View("Verify", new VerifyViewModel
        {
            Error = OpenIddictConstants.Errors.InvalidToken,
            ErrorDescription = "The specified user code is not valid. Please make sure you typed it correctly."
        });
    }
}
