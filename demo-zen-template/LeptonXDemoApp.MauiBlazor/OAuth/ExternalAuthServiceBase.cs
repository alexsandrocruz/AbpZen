using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LeptonXDemoApp.MauiBlazor.Settings;

namespace LeptonXDemoApp.MauiBlazor.OAuth;

public abstract class ExternalAuthServiceBase : IExternalAuthService
{
    protected const string AuthenticationType = "Identity.Application";

    public event Action<ClaimsPrincipal> UserChanged;

    protected ClaimsPrincipal CurrentUser { get; set; }
    protected  ILeptonXDemoAppApplicationSettingService LeptonXDemoAppApplicationSettingService { get;}

    protected ExternalAuthServiceBase(ILeptonXDemoAppApplicationSettingService leptonXDemoAppApplicationSettingService)
    {
        LeptonXDemoAppApplicationSettingService = leptonXDemoAppApplicationSettingService;
    }

    public abstract Task<LoginResult> LoginAsync(LoginInput loginInput);

    public abstract Task SignOutAsync();

    public async Task<ClaimsPrincipal> GetCurrentUser()
    {
        var accessToken = await LeptonXDemoAppApplicationSettingService.GetAccessTokenAsync();
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
