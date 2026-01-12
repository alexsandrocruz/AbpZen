using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel.OidcClient;
using Sapienza.Zen.MauiBlazor.Settings;

namespace Sapienza.Zen.MauiBlazor.OAuth
{
    public class CodeFlowExternalAuthService : ExternalAuthServiceBase
    {
        private readonly OidcClient _oidcClient;

        public CodeFlowExternalAuthService(
            ISapienzaZenApplicationSettingService leptonXDemoAppApplicationSettingService,
            OidcClient oidcClient) : base(leptonXDemoAppApplicationSettingService)
        {
            _oidcClient = oidcClient;
        }

        public override async Task<LoginResult> LoginAsync(LoginInput input)
        {
            var loginResult = await _oidcClient.LoginAsync(new LoginRequest());

            if (loginResult.IsError)
            {
                return LoginResult.Failed(loginResult.Error, loginResult.ErrorDescription);
            }

            await SapienzaZenApplicationSettingService.SetAccessTokenAsync(loginResult.AccessToken);
            CurrentUser = new ClaimsPrincipal(new ClaimsIdentity(new JwtSecurityTokenHandler().ReadJwtToken(loginResult.AccessToken).Claims, AuthenticationType));
            TriggerUserChanged();

            return LoginResult.Success();
        }

        public override async Task SignOutAsync()
        {
            var logoutResult = await _oidcClient.LogoutAsync();
            await SapienzaZenApplicationSettingService.SetAccessTokenAsync(null);

            CurrentUser = new ClaimsPrincipal(new ClaimsIdentity());
            TriggerUserChanged();
        }
    }
}
