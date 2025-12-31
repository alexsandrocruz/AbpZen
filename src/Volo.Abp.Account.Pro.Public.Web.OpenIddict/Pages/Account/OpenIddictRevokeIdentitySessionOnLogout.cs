using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenIddict.Server;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictRevokeIdentitySessionOnLogout : IOpenIddictServerHandler<OpenIddictServerEvents.HandleLogoutRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.HandleLogoutRequestContext>()
            .UseSingletonHandler<OpenIddictRevokeIdentitySessionOnLogout>()
            .SetOrder(OpenIddictServerHandlers.Session.AttachPrincipal.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    protected IdentitySessionManager IdentitySessionManager { get; }

    public OpenIddictRevokeIdentitySessionOnLogout(IdentitySessionManager identitySessionManager)
    {
        IdentitySessionManager = identitySessionManager;
    }

    public virtual async ValueTask HandleAsync(OpenIddictServerEvents.HandleLogoutRequestContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var sessionId = context.IdentityTokenHintPrincipal?.FindSessionId();
        if (sessionId != null)
        {
            context.Logger.LogDebug($"Revoking the SessionId({sessionId}).");
            await IdentitySessionManager.RevokeAsync(sessionId);
        }
        else
        {
            context.Logger.LogWarning("No SessionId was found in the token during HandleLogoutRequestContext.");
        }
    }
}
