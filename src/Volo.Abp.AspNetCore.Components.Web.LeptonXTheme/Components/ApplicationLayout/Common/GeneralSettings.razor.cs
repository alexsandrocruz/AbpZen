using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.Localization;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.Common;

public partial class GeneralSettings
{
    [Inject] public IJSRuntime JsRuntime { get; private set; }

    [Inject] public IOptions<LeptonXThemeOptions> ThemeOptions { get; private set; }

    [Inject] public IOptions<LeptonXThemeBlazorOptions> BlazorOptions { get; private set; }

    [Inject] public ILanguageProvider LanguageProvider { get; private set; }

    [Inject] public IStringLocalizerFactory LocalizerFactory { get; private set; }

    public IReadOnlyList<LanguageInfo> Languages { get; private set; } = new List<LanguageInfo>();

    public LanguageInfo CurrentLanguage { get; private set; }

    public string CurrentLanguageTwoLetters { get; private set; }

    public bool HasLanguages { get; private set; }

    public bool HasContainerWidth { get; set; }

    public bool HasMultipleStyles { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Languages = await LanguageProvider.GetLanguagesAsync();
        CurrentLanguage = await LanguagePlatformManager.GetCurrentAsync();

        if (CurrentLanguage != null && !CurrentLanguage.CultureName.IsNullOrWhiteSpace())
        {
            CurrentLanguageTwoLetters = new CultureInfo(CurrentLanguage.CultureName).TwoLetterISOLanguageName.ToUpperInvariant();
        }

        HasLanguages = Languages.Any() || CurrentLanguage == null;

        HasMultipleStyles = !ThemeOptions.Value.Styles.IsNullOrEmpty() && ThemeOptions.Value.Styles.Count > 1;
        HasContainerWidth = BlazorOptions.Value.Layout == LeptonXBlazorLayouts.SideMenu;
    }
    
    protected virtual async Task ChangeLanguageAsync(LanguageInfo language)
    {
        await LanguagePlatformManager.ChangeAsync(language);
    }
}
