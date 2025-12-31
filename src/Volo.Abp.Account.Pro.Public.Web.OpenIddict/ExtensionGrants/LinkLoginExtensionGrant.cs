using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.Account.Localization;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace Volo.Abp.Account.Web.ExtensionGrants;

public class LinkLoginExtensionGrant : TokenExtensionGrantBase
{
    public const string ExtensionGrantName = "LinkLogin";

    public const string TenantDomainParameterName = "tenant_domain";

    public override string Name => ExtensionGrantName;

    protected IdentityLinkUserManager identityLinkUserManager { get; set; }
    protected ICurrentTenant currentTenant { get; set; }
    protected ICurrentUser currentUser { get; set; }
    protected ICurrentPrincipalAccessor currentPrincipalAccessor { get; set; }
    protected IdentityUserManager userManager { get; set; }
    protected IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory { get; set; }
    protected IdentitySecurityLogManager identitySecurityLogManager { get; set; }
    protected ILogger<LinkLoginExtensionGrant> logger { get; set; }
    protected IStringLocalizer<AccountResource> localizer { get; set; }
    protected AbpAccountOpenIddictOptions accountOpenIddictOptions { get; set; }
    protected ITenantStore tenantStore { get; set; }

    [UnitOfWork]
    public override async Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        identityLinkUserManager = context.HttpContext.RequestServices.GetRequiredService<IdentityLinkUserManager>();
        currentTenant = context.HttpContext.RequestServices.GetRequiredService<ICurrentTenant>();
        currentUser = context.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
        currentPrincipalAccessor = context.HttpContext.RequestServices.GetRequiredService<ICurrentPrincipalAccessor>();
        userManager = context.HttpContext.RequestServices.GetRequiredService<IdentityUserManager>();
        userClaimsPrincipalFactory = context.HttpContext.RequestServices.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>();
        identitySecurityLogManager = context.HttpContext.RequestServices.GetRequiredService<IdentitySecurityLogManager>();
        logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LinkLoginExtensionGrant>>();
        localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<AccountResource>>();
        accountOpenIddictOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<AbpAccountOpenIddictOptions>>().Value;
        tenantStore = context.HttpContext.RequestServices.GetRequiredService<ITenantStore>();

        var transaction = await context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerFactory>().CreateTransactionAsync();
        transaction.EndpointType = OpenIddictServerEndpointType.Introspection;
        transaction.Request = new OpenIddictRequest
        {
            ClientId = context.Request.ClientId,
            ClientSecret = context.Request.ClientSecret,
            Token = context.Request.AccessToken
        };
        var notification = new OpenIddictServerEvents.ProcessAuthenticationContext(transaction);
        var dispatcher = context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerDispatcher>();
        await dispatcher.DispatchAsync(notification);

        if (notification.IsRejected)
        {
            logger.LogError("Process authentication rejected");

            return new ForbidResult(
                new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                },
                parameters: new Dictionary<string, object>
                {
                    [OpenIddictConstants.Parameters.GrantType] = context.Request.GrantType,
                }));
        }

        var principal = notification.GenericTokenPrincipal;
        if (principal == null)
        {
            logger.LogError("Process authentication principal is null");

            return new ForbidResult(
                new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidToken
                },
                parameters: new Dictionary<string, object>
                {
                    [OpenIddictConstants.Parameters.GrantType] = context.Request.GrantType,
                }));
        }

        var request = context.HttpContext.GetOpenIddictServerRequest();

        using (currentPrincipalAccessor.Change(principal.Claims))
        {
            var linkUserId = await GetRawValueOrNullAsync(request, "LinkUserId");
            if (linkUserId == null)
            {
                logger.LogError("Invalid link user id");

                return new ForbidResult(
                    new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid link user id"
                    },
                    parameters: new Dictionary<string, object>
                    {
                        [OpenIddictConstants.Parameters.GrantType] = context.Request.GrantType,
                    }));
            }

            var linkTenantId = await GetRawValueOrNullAsync(request, "LinkTenantId");
            if (linkTenantId == null && request.HasParameter("LinkTenantId"))
            {
                logger.LogError("Invalid link tenant id");

                return new ForbidResult(
                    new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid link tenant id"
                    },
                    parameters: new Dictionary<string, object>
                    {
                        [OpenIddictConstants.Parameters.GrantType] = context.Request.GrantType,
                    }));
            }

            var isLinked = await identityLinkUserManager.IsLinkedAsync(
                new IdentityLinkUserInfo(currentUser.GetId(), currentTenant.Id),
                new IdentityLinkUserInfo(linkUserId.Value, linkTenantId),
                true);

            if (isLinked)
            {
                using (currentTenant.Change(linkTenantId))
                {
                    var user = await userManager.GetByIdAsync(linkUserId.Value);
                    var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);

                    var authenticationProperties = new AuthenticationProperties();

                    if (accountOpenIddictOptions.IsTenantMultiDomain)
                    {
                        var tenantInfo = new BasicTenantInfo(null, null);
                        if (linkTenantId != null)
                        {
                            var tenantConfiguration = await tenantStore.FindAsync(linkTenantId.Value);
                            tenantInfo = new BasicTenantInfo(tenantConfiguration.Id, tenantConfiguration.Name);
                        }
                        var tenantDomain = await accountOpenIddictOptions.GetTenantDomain(context.HttpContext, tenantInfo);

                        authenticationProperties.SetParameter(TenantDomainParameterName, tenantDomain);
                    }

                    await CreateSessionAsync(context, claimsPrincipal);

                    claimsPrincipal.SetScopes(principal.GetScopes());
                    claimsPrincipal.SetResources(await GetResourcesAsync(context, principal.GetScopes()));
                    await SetClaimsDestinationsAsync(context, claimsPrincipal);

                    return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
                }
            }

            logger.LogError("The target user is not linked to you!");

            return new ForbidResult(
                new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The target user is not linked to you!"
                },
                parameters: new Dictionary<string, object>
                {
                    [OpenIddictConstants.Parameters.GrantType] = context.Request.GrantType,
                }));
        }
    }
}
