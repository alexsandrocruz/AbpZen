namespace Sapienza.EventoZen.MauiBlazor.Settings;

public interface IEventoZenApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}