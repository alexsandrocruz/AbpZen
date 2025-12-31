using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Newsletters;

public class NewsletterOptions
{
    private string _widgetViewPath;

    public string WidgetViewPath {
        get => _widgetViewPath;
        set => _widgetViewPath = Check.NotNullOrWhiteSpace(value, nameof(value));
    }

    public Dictionary<string, NewsletterPreferenceDefinition> Preferences { get; }

    public NewsletterOptions()
    {
        Preferences = new Dictionary<string, NewsletterPreferenceDefinition>();
    }

    public virtual NewsletterOptions AddPreference(
        [NotNull] string preference,
        NewsletterPreferenceDefinition definition)
    {
        Check.NotNullOrWhiteSpace(preference, nameof(preference));

        definition.WidgetViewPath ??= _widgetViewPath;
        definition.Preference = preference;

        Preferences.Add(preference, definition);

        return this;
    }
}
