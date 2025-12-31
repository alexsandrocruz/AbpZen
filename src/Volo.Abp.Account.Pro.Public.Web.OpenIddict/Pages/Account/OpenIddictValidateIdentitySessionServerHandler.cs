using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity.Session;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictValidateIdentitySessionServerHandler : IOpenIddictServerHandler<OpenIddictServerEvents.ValidateTokenContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateTokenContext>()
            .UseScopedHandler<OpenIddictValidateIdentitySessionServerHandler>()
            .SetOrder(OpenIddictServerHandlers.Protection.ValidatePrincipal.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    protected IdentitySessionChecker IdentitySessionChecker { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public OpenIddictValidateIdentitySessionServerHandler(IdentitySessionChecker identitySessionChecker, ICurrentTenant currentTenant)
    {
        IdentitySessionChecker = identitySessionChecker;
        CurrentTenant = currentTenant;
    }

    public virtual async ValueTask HandleAsync(OpenIddictServerEvents.ValidateTokenContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var sessionId = context.Principal?.FindSessionId();
        if (sessionId.IsNullOrWhiteSpace())
        {
            context.Logger.LogWarning("No SessionId was found in the token during ValidateTokenContext.");
            return;
        }

        using (CurrentTenant.Change(context.Principal.FindTenantId()))
        {
            if (!await IdentitySessionChecker.IsValidateAsync(sessionId))
            {
                context.Logger.LogWarning("The token is no longer valid because the user's session expired.");
                context.Reject(error: AbpExceptionHandlingConsts.InvalidToken, description: AbpExceptionHandlingConsts.SessionExpired);
            }
        }
    }
}
