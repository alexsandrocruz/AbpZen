using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.LeptonX.Shared.Localization;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.Common;

public partial class MobileGeneralSettings
{
    [Inject] public ILanguagePlatformManager LanguagePlatformManager { get; set; }

    [Inject] public ILanguageProvider LanguageProvider { get; set; }

    [Inject] public IStringLocalizer<LeptonXResource> L { get; set; }

    [Inject] public IOptions<LeptonXThemeOptions> ThemeOptions { get; private set; }

    public IReadOnlyList<LanguageInfo> Languages { get; private set; } = new List<LanguageInfo>();

    public LanguageInfo CurrentLanguage { get; private set; }

    public bool HasLanguages { get; private set; }

    public bool HasMultipleStyles { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        Languages = await LanguageProvider.GetLanguagesAsync();
        CurrentLanguage = await LanguagePlatformManager.GetCurrentAsync();

        HasLanguages = Languages.Any() || CurrentLanguage == null;

        // TODO: Enable it after resolution of https://github.com/volosoft/lepton/issues/670
        // HasMultipleStyles = !ThemeOptions.Value.Styles.IsNullOrEmpty() && ThemeOptions.Value.Styles.Count > 1;
        HasMultipleStyles = true;
    }
    
    protected virtual async Task ChangeLanguageAsync(LanguageInfo language)
    {
        await LanguagePlatformManager.ChangeAsync(language);
    }
}
