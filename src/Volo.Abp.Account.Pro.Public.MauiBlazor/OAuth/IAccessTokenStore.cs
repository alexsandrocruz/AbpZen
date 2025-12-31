namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public interface IAccessTokenStore
{
    Task<string> GetAccessTokenAsync();

    Task SetAccessTokenAsync(string? accessToken);
}
