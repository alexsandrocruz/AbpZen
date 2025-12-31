using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;

namespace Volo.Abp.LanguageManagement;

public interface IDynamicResourceLocalizer
{
    LocalizedString GetOrNull(LocalizationResourceBase resource, string cultureName, string name);

    void Fill(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary);
    
    Task FillAsync(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary);
}
