using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenIddict.Server;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictRevokeIdentitySessionOnRevocation : IOpenIddictServerHandler<OpenIddictServerEvents.HandleRevocationRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.HandleRevocationRequestContext>()
            .UseSingletonHandler<OpenIddictRevokeIdentitySessionOnRevocation>()
            .SetOrder(OpenIddictServerHandlers.Revocation.RevokeToken.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    protected IdentitySessionManager IdentitySessionManager { get; }

    public OpenIddictRevokeIdentitySessionOnRevocation(IdentitySessionManager identitySessionManager)
    {
        IdentitySessionManager = identitySessionManager;
    }

    public virtual async ValueTask HandleAsync(OpenIddictServerEvents.HandleRevocationRequestContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var sessionId = context.Principal.FindSessionId();
        if (sessionId != null)
        {
            context.Logger.LogDebug($"Revoking the SessionId({sessionId}).");
            await IdentitySessionManager.RevokeAsync(sessionId);
        }
        else
        {
            context.Logger.LogWarning("No SessionId was found in the token during HandleRevocationRequestContext.");
        }
    }
}
