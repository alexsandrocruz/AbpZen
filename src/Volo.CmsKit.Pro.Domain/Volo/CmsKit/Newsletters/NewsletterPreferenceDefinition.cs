using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Newsletters;

public class NewsletterPreferenceDefinition
{
    public string Preference { get; set; }

    [NotNull]
    public ILocalizableString DisplayPreference { get; }

    [CanBeNull]
    public ILocalizableString PrivacyPolicyConfirmation { get; set; }

    [CanBeNull]
    public ILocalizableString Definition { get; set; }

    public List<string> AdditionalPreferences { get; set; }

    [CanBeNull]
    public string WidgetViewPath { get; set; }

    public NewsletterPreferenceDefinition(
        [NotNull] ILocalizableString displayPreference,
        [CanBeNull] ILocalizableString definition = null,
        [CanBeNull] ILocalizableString privacyPolicyConfirmation = null,
        [CanBeNull] List<string> additionalPreferences = null,
        [CanBeNull] string widgetViewPath = null)
    {
        DisplayPreference = Check.NotNull(displayPreference, nameof(displayPreference));
        Definition = definition;
        PrivacyPolicyConfirmation = privacyPolicyConfirmation;
        AdditionalPreferences = additionalPreferences;
        WidgetViewPath = widgetViewPath;
    }
}
