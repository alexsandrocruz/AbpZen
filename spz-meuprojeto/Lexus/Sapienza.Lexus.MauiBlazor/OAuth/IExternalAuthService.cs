using System.Security.Claims;

namespace Sapienza.Lexus.MauiBlazor.OAuth;

public interface IExternalAuthService
{
      event Action<ClaimsPrincipal> UserChanged;

      Task<LoginResult> LoginAsync(LoginInput loginInput);

      Task SignOutAsync();

      Task<ClaimsPrincipal> GetCurrentUser();
}
