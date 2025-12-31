using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictCreateIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignInContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessSignInContext>()
            .UseSingletonHandler<OpenIddictCreateIdentitySession>()
            .SetOrder(100_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    protected IdentitySessionManager IdentitySessionManager { get; }
    protected IWebClientInfoProvider WebClientInfoProvider { get; }
    protected IOptions<AbpAccountOpenIddictOptions> Options { get; }

    public OpenIddictCreateIdentitySession(IdentitySessionManager identitySessionManager, IWebClientInfoProvider webClientInfoProvider, IOptions<AbpAccountOpenIddictOptions> options)
    {
        IdentitySessionManager = identitySessionManager;
        WebClientInfoProvider = webClientInfoProvider;
        Options = options;
    }

    public async ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignInContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.IsRequestHandled || context.IsRequestSkipped || context.IsRejected || context.Principal == null || context.Request.IsClientCredentialsGrantType())
        {
            return;
        }

        var sessionId = context.Principal.FindSessionId();
        
        if (sessionId == null)
        {
            context.Logger.LogError("SessionId is null. It's not possible to save the session during the sign-in process.");
            return;
        }

        var device = IdentitySessionDevices.OAuth;

        if (!context.ClientId.IsNullOrWhiteSpace() &&
            Options.Value.ClientIdToDeviceMap.TryGetValue(context.ClientId, out var clientDevice))
        {
            device = clientDevice;
        }

        await IdentitySessionManager.CreateAsync(
            sessionId,
            device,
            WebClientInfoProvider.DeviceInfo,
            context.Principal.FindUserId()!.Value,
            context.Principal.FindTenantId(),
            context.ClientId,
            WebClientInfoProvider.ClientIpAddress);
    }
}
