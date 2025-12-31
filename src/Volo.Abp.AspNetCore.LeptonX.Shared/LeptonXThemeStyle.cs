using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.LeptonX.Shared;

public class LeptonXThemeStyle
{
    public LocalizableString DisplayName { get; set; }

    [CanBeNull]
    public string Icon { get; set; }

    public LeptonXThemeStyle(LocalizableString displayName, [CanBeNull] string icon = null)
    {
        DisplayName = displayName;
        Icon = icon;
    }
}
