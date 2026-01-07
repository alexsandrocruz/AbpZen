namespace Sapienza.Cursos.MauiBlazor.Settings;

public interface ICursosApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}