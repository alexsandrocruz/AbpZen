using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme.Navigation;

[ExposeServices(typeof(LanguageMauiBlazorManager), typeof(ILanguagePlatformManager))]
public class LanguageMauiBlazorManager : ILanguagePlatformManager, ITransientDependency
{
    private IJSRuntime JsRuntime { get; }

    private ILanguageProvider LanguageProvider { get; }
    
    private NavigationManager NavigationManager { get; }

    public LanguageMauiBlazorManager(
        IJSRuntime jsRuntime,
        ILanguageProvider languageProvider, 
        NavigationManager navigationManager)
    {
        JsRuntime = jsRuntime;
        LanguageProvider = languageProvider;
        NavigationManager = navigationManager;
    }

    public virtual async Task ChangeAsync(LanguageInfo newLanguage)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(newLanguage.CultureName);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(newLanguage.UiCultureName);

        Preferences.Set("Abp.SelectedLanguage", newLanguage.CultureName);
        await JsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            "Abp.SelectedLanguage", newLanguage.UiCultureName
        );

        await JsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            "Abp.IsRtl", CultureHelper.IsRtl
        );

        NavigationManager.NavigateTo(NavigationManager.Uri, true);
    }

    public virtual async Task<LanguageInfo> GetCurrentAsync()
    {
        var selectedLanguageName = Preferences.Get("Abp.SelectedLanguage", string.Empty);

        var languages = await LanguageProvider.GetLanguagesAsync();

        LanguageInfo currentLanguage = null;

        if (!selectedLanguageName.IsNullOrWhiteSpace())
        {
            currentLanguage = languages.FirstOrDefault(l => l.UiCultureName == selectedLanguageName);
        }

        if (currentLanguage == null)
        {
            currentLanguage = languages.FirstOrDefault(l => l.UiCultureName == CultureInfo.CurrentUICulture.Name);
        }

        if (currentLanguage == null)
        {
            currentLanguage = languages.FirstOrDefault();
        }

        return currentLanguage;
    }
    
    public virtual Task InitializeAsync()
    {
        var selectedLanguageName = Preferences.Get("Abp.SelectedLanguage", string.Empty);

        if (!selectedLanguageName.IsNullOrWhiteSpace())
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(selectedLanguageName);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(selectedLanguageName);
        }
        
        return Task.CompletedTask;
    }
    
}