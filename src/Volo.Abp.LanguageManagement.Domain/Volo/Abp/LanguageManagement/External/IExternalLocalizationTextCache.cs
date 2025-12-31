using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.LanguageManagement.External;

public interface IExternalLocalizationTextCache
{
    Dictionary<string, string>? TryGetTextsFromCache(string resourceName, string cultureName);
    
    Task<Dictionary<string, string>> GetTextsAsync(
        string resourceName,
        string cultureName,
        Func<Task<Dictionary<string, string>>> factory
    );

    Task InvalidateAsync(string resourceName, string cultureName);
}