using Microsoft.AspNetCore.Components.Authorization;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public class ExternalAuthStateProvider : AuthenticationStateProvider
{
    private AuthenticationState _currentUser;
    private readonly IExternalAuthService _externalAuthService;

    public ExternalAuthStateProvider(IExternalAuthService externalAuthService)
    {
        _externalAuthService = externalAuthService;
        externalAuthService.UserChanged += (newUser) =>
        {
            _currentUser = new AuthenticationState(newUser);
            NotifyAuthenticationStateChanged(Task.FromResult(_currentUser));
        };
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return _currentUser ??= new AuthenticationState(await _externalAuthService.GetCurrentUser());
    }
}
