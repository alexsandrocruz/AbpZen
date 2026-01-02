namespace LeptonXDemoApp.MauiBlazor.Settings;

public interface ILeptonXDemoAppApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}