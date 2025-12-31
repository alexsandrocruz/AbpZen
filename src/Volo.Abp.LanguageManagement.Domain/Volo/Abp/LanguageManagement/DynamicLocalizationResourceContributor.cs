using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;

namespace Volo.Abp.LanguageManagement;

public class DynamicLocalizationResourceContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => true;

    protected LocalizationResourceBase Resource { get; private set; }
    protected IDynamicResourceLocalizer DynamicResourceLocalizer { get; private set; }

    public void Initialize(LocalizationResourceInitializationContext context)
    {
        Resource = context.Resource;
        DynamicResourceLocalizer = context.ServiceProvider.GetRequiredService<IDynamicResourceLocalizer>();
    }

    public LocalizedString GetOrNull(string cultureName, string name)
    {
        return DynamicResourceLocalizer.GetOrNull(Resource, cultureName, name);
    }

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        DynamicResourceLocalizer.Fill(Resource, cultureName, dictionary);
    }

    public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        return DynamicResourceLocalizer.FillAsync(Resource, cultureName, dictionary);
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        /* Dynamic contributor should return empty
         * Because this method's purpose is to get statically supported cultures by this provider.
         */
        return Task.FromResult((IEnumerable<string>)Array.Empty<string>());
    }
}