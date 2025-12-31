using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

[DependencyInjection.Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IAbpAccessTokenProvider))]
public class MauiAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    private readonly IAccessTokenStore _accessTokenStore;

    public MauiAccessTokenProvider(IAccessTokenStore accessTokenStore)
    {
        _accessTokenStore = accessTokenStore;
    }

    public async Task<string> GetTokenAsync()
    {
        return await _accessTokenStore.GetAccessTokenAsync();
    }
}