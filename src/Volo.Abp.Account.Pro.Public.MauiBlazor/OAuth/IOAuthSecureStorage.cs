namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public interface IOAuthSecureStorage
{
    Task SetAsync(string key, string value);
    
    Task<string> GetAsync(string key);
    
    Task RemoveAsync(string key);
}