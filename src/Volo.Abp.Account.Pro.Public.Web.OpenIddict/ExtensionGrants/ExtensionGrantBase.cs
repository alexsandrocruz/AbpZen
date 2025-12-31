using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace Volo.Abp.Account.Web.ExtensionGrants;

public abstract class TokenExtensionGrantBase : ITokenExtensionGrant
{
    protected virtual Task<Guid?> GetRawValueOrNullAsync(OpenIddictRequest request, string key)
    {
        var str = request.GetParameter(key).ToString();
        if (str.IsNullOrWhiteSpace())
        {
            return Task.FromResult<Guid?>(null);
        }

        return Guid.TryParse(str, out var guid) ? Task.FromResult<Guid?>(guid) : Task.FromResult<Guid?>(null);
    }

    protected virtual async Task<IEnumerable<string>> GetResourcesAsync(ExtensionGrantContext context, ImmutableArray<string> scopes)
    {
        var resources = new List<string>();
        if (!scopes.Any())
        {
            return resources;
        }

        await foreach (var resource in context.HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>().ListResourcesAsync(scopes))
        {
            resources.Add(resource);
        }
        return resources;
    }

    protected virtual async Task SetClaimsDestinationsAsync(ExtensionGrantContext context, ClaimsPrincipal principal)
    {
        await context.HttpContext.RequestServices.GetRequiredService<AbpOpenIddictClaimsPrincipalManager>().HandleAsync(context.Request, principal);
    }

    protected virtual async Task CreateSessionAsync(ExtensionGrantContext context, ClaimsPrincipal principal)
    {
        var identitySessionManager = context.HttpContext.RequestServices.GetRequiredService<IdentitySessionManager>();
        var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<AbpAccountOpenIddictOptions>>();
        var webClientInfoProvider = context.HttpContext.RequestServices.GetRequiredService<IWebClientInfoProvider>();

        var sessionId = principal.FindSessionId();
        if (sessionId != null)
        {
            var device = IdentitySessionDevices.OAuth;
            if (!context.Request.ClientId.IsNullOrWhiteSpace() && options.Value.ClientIdToDeviceMap.TryGetValue(context.Request.ClientId, out var clientDevice))
            {
                device = clientDevice;
            }

            await identitySessionManager.CreateAsync(
                sessionId,
                device,
                webClientInfoProvider.DeviceInfo,
                principal.FindUserId()!.Value,
                principal.FindTenantId(),
                context.Request.ClientId,
                webClientInfoProvider.ClientIpAddress);
        }
    }

    public abstract Task<IActionResult> HandleAsync(ExtensionGrantContext context);

    public abstract string Name { get; }
}
