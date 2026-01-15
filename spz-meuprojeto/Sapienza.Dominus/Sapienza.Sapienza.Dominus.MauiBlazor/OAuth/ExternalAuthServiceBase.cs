using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Sapienza.Sapienza.Dominus.MauiBlazor.Settings;

namespace Sapienza.Sapienza.Dominus.MauiBlazor.OAuth;

public abstract class ExternalAuthServiceBase : IExternalAuthService
{
    protected const string AuthenticationType = "Identity.Application";

    public event Action<ClaimsPrincipal> UserChanged;

    protected ClaimsPrincipal CurrentUser { get; set; }
    protected  ISapienza.DominusApplicationSettingService Sapienza.DominusApplicationSettingService { get;}

    protected ExternalAuthServiceBase(ISapienza.DominusApplicationSettingService leptonXDemoAppApplicationSettingService)
    {
        Sapienza.DominusApplicationSettingService = leptonXDemoAppApplicationSettingService;
    }

    public abstract Task<LoginResult> LoginAsync(LoginInput loginInput);

    public abstract Task SignOutAsync();

    public async Task<ClaimsPrincipal> GetCurrentUser()
    {
        var accessToken = await Sapienza.DominusApplicationSettingService.GetAccessTokenAsync();
        if (!accessToken.IsNullOrWhiteSpace())
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                CurrentUser = new ClaimsPrincipal(new ClaimsIdentity(new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims, AuthenticationType));
            }
        }

        return CurrentUser ?? new ClaimsPrincipal();
    }

    protected void TriggerUserChanged()
    {
        UserChanged?.Invoke(CurrentUser);
    }
}
