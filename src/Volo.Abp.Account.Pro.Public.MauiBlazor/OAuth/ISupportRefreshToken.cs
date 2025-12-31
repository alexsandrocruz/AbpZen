namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public interface ISupportRefreshToken
{
    Task<string> GetRefreshTokenAsync();
    
    Task<DateTime?> GetAccessTokenExpiredTimeAsync();

    Task SetRefreshTokenAsync(string refreshToken, string accessTokenExpiredTime);
}