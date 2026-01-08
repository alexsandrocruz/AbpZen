namespace Sapienza.Lexus.MauiBlazor.Settings;

public interface ILexusApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}