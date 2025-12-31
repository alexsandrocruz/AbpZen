using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;

namespace Volo.Abp.Account.Web.ExtensionGrants;

public class LinkLoginExtensionGrantProcessJsonResponse : IOpenIddictServerHandler<OpenIddictServerEvents.ApplyTokenResponseContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ApplyTokenResponseContext>()
            .AddFilter<OpenIddictServerAspNetCoreHandlerFilters.RequireHttpRequest>()
            .UseSingletonHandler<LinkLoginExtensionGrantProcessJsonResponse>()
            .SetOrder(OpenIddictServerAspNetCoreHandlers.ProcessJsonResponse<OpenIddictServerEvents.ApplyTokenResponseContext>.Descriptor.Order - 1)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public ValueTask HandleAsync(OpenIddictServerEvents.ApplyTokenResponseContext context)
    {
        Check.NotNull(context, nameof(context));
        Check.NotNull(context.Transaction.Response, nameof(context.Transaction.Response));

        if (context.Transaction.Properties.ContainsKey(typeof(AuthenticationProperties).FullName!))
        {
            var properties = context.Transaction.Properties[typeof(AuthenticationProperties).FullName!]?.As<AuthenticationProperties>();
            if (properties != null && properties.Parameters.ContainsKey(LinkLoginExtensionGrant.TenantDomainParameterName))
            {
                context.Transaction.Response.AddParameter(
                    LinkLoginExtensionGrant.TenantDomainParameterName,
                    new OpenIddictParameter(properties.GetParameter<string>(LinkLoginExtensionGrant.TenantDomainParameterName))
                );
            }
        }
        
        return default;
    }
}
