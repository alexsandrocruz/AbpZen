using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(ProtocolCallbackModel))]
public class OpenIddictProtocolCallbackModel : ProtocolCallbackModel
{
    protected IOpenIddictApplicationManager OpenIddictApplicationManager { get; }
    protected IOpenIddictTokenManager OpenIddictTokenManager { get; }
    protected AbpOpenIddictIdentifierConverter IdentifierConverter { get; }

    public OpenIddictProtocolCallbackModel(
        IOpenIddictApplicationManager openIddictApplicationManager,
        IOpenIddictTokenManager openIddictTokenManager,
        AbpOpenIddictIdentifierConverter identifierConverter)
    {
        OpenIddictApplicationManager = openIddictApplicationManager;
        OpenIddictTokenManager = openIddictTokenManager;
        IdentifierConverter = identifierConverter;
    }

    public async override Task<IActionResult> OnGetAsync(string protocol)
    {
        var code = Request.Query["code"].ToString();
        if (code.IsNullOrEmpty())
        {
            Logger.LogWarning("Code is not specified!");
            return RedirectToPage("/Account/Login");
        }

        var token = (await OpenIddictTokenManager.FindByReferenceIdAsync(code, HttpContext.RequestAborted))!.As<OpenIddictTokenModel>();
        if (token.ApplicationId == null)
        {
            Logger.LogWarning("Token's ApplicationId is null!");
            return RedirectToPage("/Account/Login");
        }

        var application = await OpenIddictApplicationManager.FindByIdAsync(IdentifierConverter.ToString(token.ApplicationId.Value), HttpContext.RequestAborted);
        if (application == null)
        {
            Logger.LogWarning("Token's ApplicationId is not found!");
            return RedirectToPage("/Account/Login");
        }

        var redirectUris = await OpenIddictApplicationManager.GetRedirectUrisAsync(application, HttpContext.RequestAborted);
        if (!redirectUris.Any(x => x.EndsWith(protocol) ))
        {
            Logger.LogWarning($"The redirect uri({protocol}) is not valid!");
            return RedirectToPage("/Account/Login");
        }

        return await base.OnGetAsync(protocol);
    }
}
