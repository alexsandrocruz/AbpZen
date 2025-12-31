using Microsoft.AspNetCore.Components.Authorization;

namespace LeptonXDemoApp.MauiBlazor.OAuth;

public class ExternalAuthStateProvider : AuthenticationStateProvider
{
    private AuthenticationState _currentUser;
    private readonly IExternalAuthService _externalAuthService;

    public ExternalAuthStateProvider(IExternalAuthService codeFlowExternalAuthService)
    {
        _externalAuthService = codeFlowExternalAuthService;
        codeFlowExternalAuthService.UserChanged += (newUser) =>
        {
            _currentUser = new AuthenticationState(newUser);
            NotifyAuthenticationStateChanged(Task.FromResult(_currentUser));
        };
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return _currentUser ??= new AuthenticationState(await _externalAuthService.GetCurrentUser());
    }
}
