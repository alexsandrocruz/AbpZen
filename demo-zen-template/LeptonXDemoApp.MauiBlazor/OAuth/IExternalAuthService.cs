using System.Security.Claims;

namespace LeptonXDemoApp.MauiBlazor.OAuth;

public interface IExternalAuthService
{
      event Action<ClaimsPrincipal> UserChanged;

      Task<LoginResult> LoginAsync(LoginInput loginInput);

      Task SignOutAsync();

      Task<ClaimsPrincipal> GetCurrentUser();
}
