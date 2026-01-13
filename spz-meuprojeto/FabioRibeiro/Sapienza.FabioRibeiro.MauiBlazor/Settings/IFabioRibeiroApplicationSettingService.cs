namespace Sapienza.FabioRibeiro.MauiBlazor.Settings;

public interface IFabioRibeiroApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}