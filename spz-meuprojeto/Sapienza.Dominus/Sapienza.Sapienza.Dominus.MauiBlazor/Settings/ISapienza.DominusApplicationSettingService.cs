namespace Sapienza.Dominus.MauiBlazor.Settings;

public interface IDominusApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}