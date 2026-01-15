namespace Sapienza.Sapienza.Dominus.MauiBlazor.Settings;

public interface ISapienza.DominusApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}