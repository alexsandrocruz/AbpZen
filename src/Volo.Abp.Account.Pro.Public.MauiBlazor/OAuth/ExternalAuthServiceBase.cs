using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public abstract class ExternalAuthServiceBase : IExternalAuthService
{
    protected const string AuthenticationType = "Identity.Application";

    public event Action<ClaimsPrincipal> UserChanged;
    protected  IAccessTokenStore AccessTokenStore { get;}

    protected ExternalAuthServiceBase(IAccessTokenStore accessTokenStore)
    {
        AccessTokenStore = accessTokenStore;
    }

    public abstract Task<LoginResult> LoginAsync(LoginInput loginInput);

    public abstract Task SignOutAsync();

    protected void TriggerUserChanged(ClaimsPrincipal newUser)
    {
        UserChanged?.Invoke(newUser);
    }
    
    public async Task<ClaimsPrincipal> GetCurrentUser()
    {
        var accessToken = await AccessTokenStore.GetAccessTokenAsync();
        if (!accessToken.IsNullOrWhiteSpace())
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return new ClaimsPrincipal(new ClaimsIdentity(new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims, AuthenticationType));
            }
        }

        return new ClaimsPrincipal(new ClaimsIdentity());
    }
}
