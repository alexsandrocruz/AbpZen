using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public class DefaultOAuthSecureStorage : IOAuthSecureStorage, ITransientDependency
{
    public Task SetAsync(string key, string value)
    {
        Preferences.Set(key, value);
        return Task.CompletedTask;
    }

    public Task<string> GetAsync(string key)
    {
        return Task.FromResult(Preferences.Get(key, string.Empty));
    }

    public Task RemoveAsync(string key)
    {
        Preferences.Remove(key);
        return Task.CompletedTask;
    }
}