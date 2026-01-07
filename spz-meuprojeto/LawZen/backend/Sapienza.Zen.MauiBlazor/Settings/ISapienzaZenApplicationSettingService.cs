namespace Sapienza.Zen.MauiBlazor.Settings;

public interface ISapienzaZenApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}