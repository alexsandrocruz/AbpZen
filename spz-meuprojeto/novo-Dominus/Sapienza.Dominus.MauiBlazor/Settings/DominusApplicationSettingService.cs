using Volo.Abp.DependencyInjection;

namespace Sapienza.Dominus.MauiBlazor.Settings;

public class DominusApplicationSettingService : IDominusApplicationSettingService, ITransientDependency
{
    private const string AccessTokenKey = "access_token";

    public  Task<string> GetAccessTokenAsync()
    {
        return Task.FromResult(Preferences.Get(AccessTokenKey, string.Empty));
    }

    public Task SetAccessTokenAsync(string accessToken)
    {
        if (accessToken.IsNullOrWhiteSpace())
        {
            Preferences.Remove(AccessTokenKey);
        }
        else
        {
            Preferences.Set(AccessTokenKey, accessToken);
        }

        return Task.CompletedTask;
    }
}
