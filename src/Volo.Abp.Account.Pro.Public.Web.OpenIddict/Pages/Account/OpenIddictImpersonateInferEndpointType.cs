using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictImpersonateInferEndpointType : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessRequestContext>()
            .AddFilter<OpenIddictServerAspNetCoreHandlerFilters.RequireHttpRequest>()
            .UseSingletonHandler<OpenIddictImpersonateInferEndpointType>()
            .SetOrder(OpenIddictServerHandlers.InferEndpointType.Descriptor.Order + 1)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    private readonly static List<string> ImpersonateUris = new List<string>()
    {
        "Account/ImpersonateTenant",
        "Account/ImpersonateUser",
        "Account/BackToImpersonator",
        "Account/DelegatedImpersonate"
    };

    public virtual ValueTask HandleAsync(OpenIddictServerEvents.ProcessRequestContext context)
    {
        Check.NotNull(context, nameof(context));
        var request = context.Transaction.GetHttpRequest();
        Check.NotNull(request, nameof(request));

        if (context.EndpointType == OpenIddictServerEndpointType.Authorization
            && request!.HasFormContentType &&
            context.RequestUri != null &&
            ImpersonateUris.Any(impersonateUri => context.RequestUri.PathAndQuery.Contains(impersonateUri, StringComparison.OrdinalIgnoreCase)))
        {
            context.SkipRequest();
        }

        return default;
    }
}
