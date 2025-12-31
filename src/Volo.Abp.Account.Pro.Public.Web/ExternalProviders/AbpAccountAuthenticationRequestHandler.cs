using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Account.Public.Web.ExternalProviders;

public class AbpAccountAuthenticationRequestHandler<TOptions, THandler> : IAuthenticationRequestHandler, IAuthenticationSignInHandler

    where TOptions : RemoteAuthenticationOptions, new()
    where THandler : RemoteAuthenticationHandler<TOptions>
{
    protected readonly THandler InnerHandler;
    protected readonly IOptions<TOptions> OptionsManager;

    public AbpAccountAuthenticationRequestHandler(THandler innerHandler, IOptions<TOptions> optionsManager)
    {
        InnerHandler = innerHandler;
        OptionsManager = optionsManager;
    }

    public virtual async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        await InnerHandler.InitializeAsync(scheme, context);
    }

    public virtual async Task<AuthenticateResult> AuthenticateAsync()
    {
        return await InnerHandler.AuthenticateAsync();
    }

    public virtual async Task ChallengeAsync(AuthenticationProperties properties)
    {
        await SetOptionsAsync();

        await InnerHandler.ChallengeAsync(properties);
    }

    public virtual async Task ForbidAsync(AuthenticationProperties properties)
    {
        await InnerHandler.ForbidAsync(properties);
    }

    public async Task SignOutAsync(AuthenticationProperties properties)
    {
        var signOutHandler = InnerHandler as IAuthenticationSignOutHandler;
        if (signOutHandler == null)
        {
            throw new InvalidOperationException($"The authentication handler registered for scheme '{InnerHandler.Scheme}' is '{InnerHandler.GetType().Name}' which cannot be used for SignOutAsync");
        }
        
        await SetOptionsAsync();
        await signOutHandler.SignOutAsync(properties);
    }

    public async Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
    {
        var signInHandler = InnerHandler as IAuthenticationSignInHandler;
        if (signInHandler == null)
        {
            throw new InvalidOperationException($"The authentication handler registered for scheme '{InnerHandler.Scheme}' is '{InnerHandler.GetType().Name}' which cannot be used for SignInAsync");
        }
        
        await SetOptionsAsync();
        await signInHandler.SignInAsync(user, properties);
    }

    public virtual async Task<bool> HandleRequestAsync()
    {
        if (await InnerHandler.ShouldHandleRequestAsync())
        {
            await SetOptionsAsync();
        }

        return await InnerHandler.HandleRequestAsync();
    }

    public virtual THandler GetHandler()
    {
        return InnerHandler;
    }
    
    protected virtual async Task SetOptionsAsync()
    {
        await OptionsManager.SetAsync(InnerHandler.Scheme.Name);
        ObjectHelper.TrySetProperty(InnerHandler, handler => handler.Options, () => OptionsManager.Value);
    }
}
