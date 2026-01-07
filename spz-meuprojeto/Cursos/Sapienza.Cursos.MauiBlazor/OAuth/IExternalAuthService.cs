using System.Security.Claims;

namespace Sapienza.Cursos.MauiBlazor.OAuth;

public interface IExternalAuthService
{
      event Action<ClaimsPrincipal> UserChanged;

      Task<LoginResult> LoginAsync(LoginInput loginInput);

      Task SignOutAsync();

      Task<ClaimsPrincipal> GetCurrentUser();
}
