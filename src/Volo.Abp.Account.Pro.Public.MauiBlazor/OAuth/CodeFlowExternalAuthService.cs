using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel.OidcClient;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth
{
    public class CodeFlowExternalAuthService : ExternalAuthServiceBase
    {
        private readonly OidcClient _oidcClient;

        public CodeFlowExternalAuthService(
            IAccessTokenStore accessTokenStore,
            OidcClient oidcClient) : base(accessTokenStore)
        {
            _oidcClient = oidcClient;
        }

        public async override Task<LoginResult> LoginAsync(LoginInput input)
        {
            var loginResult = await _oidcClient.LoginAsync(new LoginRequest());

            if (loginResult.IsError)
            {
                return LoginResult.Failed(loginResult.Error, loginResult.ErrorDescription);
            }

            await AccessTokenStore.SetAccessTokenAsync(loginResult.AccessToken);
            
            if(AccessTokenStore is ISupportRefreshToken supportRefreshToken)
            {
                await supportRefreshToken.SetRefreshTokenAsync(loginResult.RefreshToken,loginResult.AccessTokenExpiration.ToString());
            }
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new JwtSecurityTokenHandler().ReadJwtToken(loginResult.AccessToken).Claims, AuthenticationType));
            TriggerUserChanged(user);

            return LoginResult.Success();
        }

        public async override Task SignOutAsync()
        {
            var logoutResult = await _oidcClient.LogoutAsync();
            await AccessTokenStore.SetAccessTokenAsync(null);
            if(AccessTokenStore is ISupportRefreshToken supportRefreshToken)
            {
                await supportRefreshToken.SetRefreshTokenAsync(null,null);
            }

            TriggerUserChanged(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}
