using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public class AccessTokenStore : IAccessTokenStore, ISupportRefreshToken, ITransientDependency
{
    public const string HttpClientName = "AccessTokenStoreClient";
    
    protected const string AccessTokenKey = "access_token";
    protected const string RefreshTokenKey = "refresh_token";
    protected const string ExpiredTimeKey = "expired_time";
    
    protected IHttpClientFactory HttpClientFactory { get; }
    protected OAuthConfigOptions OAuthConfigOptions { get; }
    protected ICurrentTenant CurrentTenant { get; }
    
    protected IOAuthSecureStorage SecureStorage { get; }
    
    public AccessTokenStore(
        IHttpClientFactory httpClientFactory,
        IOptions<OAuthConfigOptions> oAuthConfigOptions,
        ICurrentTenant currentTenant, 
        IOAuthSecureStorage secureStorage)
    {
        HttpClientFactory = httpClientFactory;
        OAuthConfigOptions = oAuthConfigOptions.Value;
        CurrentTenant = currentTenant;
        SecureStorage = secureStorage;
    }

    public async Task SetAccessTokenAsync(string accessToken)
    {
        if (accessToken.IsNullOrWhiteSpace())
        {
            await SecureStorage.RemoveAsync(AccessTokenKey);
            return;
        }
        
        await SecureStorage.SetAsync(AccessTokenKey, accessToken);
    }

    public async Task SetRefreshTokenAsync(string refreshToken, string accessTokenExpiredTime)
    {
        if (refreshToken.IsNullOrWhiteSpace())
        {
            await SecureStorage.RemoveAsync(RefreshTokenKey);
            await SecureStorage.RemoveAsync(ExpiredTimeKey);
            return;
        }
        
        await SecureStorage.SetAsync(RefreshTokenKey, refreshToken);
        await SecureStorage.SetAsync(ExpiredTimeKey, accessTokenExpiredTime);
    }

    public async Task<string> GetAccessTokenAsync()
    {
        await TryRefreshAccessTokenAsync();
        return await SecureStorage.GetAsync(AccessTokenKey);
    }

    public Task<string> GetRefreshTokenAsync()
    {
        return SecureStorage.GetAsync(RefreshTokenKey);
    }

    public async Task<DateTime?> GetAccessTokenExpiredTimeAsync()
    {
        var expiredTime = await SecureStorage.GetAsync(ExpiredTimeKey);
        if (expiredTime.IsNullOrWhiteSpace() || !DateTime.TryParse(expiredTime, out var result))
        {
            return null;
        }

        return result;
    }
    
    protected virtual async Task TryRefreshAccessTokenAsync()
    {
        var expiredTime = await GetAccessTokenExpiredTimeAsync();
        if (expiredTime == null || DateTime.Now < expiredTime)
        {
            return;
        }
        
        var refreshToken = await GetRefreshTokenAsync();
        if (refreshToken.IsNullOrWhiteSpace())
        {
            return;
        }
        
        var client = HttpClientFactory.CreateClient(HttpClientName);
        var discoveryDocument = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = OAuthConfigOptions.Authority,
            Policy = new DiscoveryPolicy
            {
                RequireHttps = OAuthConfigOptions.RequireHttpsMetadata
            }
        });

        var refreshTokenRequest = new RefreshTokenRequest {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = OAuthConfigOptions.ClientId,
            ClientSecret = OAuthConfigOptions.ClientSecret,
            RefreshToken = refreshToken
        };
        
        if (CurrentTenant.IsAvailable)
        {
            refreshTokenRequest.Headers.Add("__tenant", CurrentTenant.Id.ToString());
        }
        
        var tokenResponse = await client.RequestRefreshTokenAsync(refreshTokenRequest);
        
        if(tokenResponse.IsError)
        {
            return;
        }
        
        await SetAccessTokenAsync(tokenResponse.AccessToken);
        await SetRefreshTokenAsync(tokenResponse.RefreshToken, DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString());
    }
}