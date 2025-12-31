using Microsoft.JSInterop;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme.Navigation
{
    [ExposeServices(typeof(ILanguagePlatformManager))]
    public class LanguageBlazorWasmManager : ILanguagePlatformManager, ITransientDependency
    {
        protected IJSRuntime JsRuntime { get; }

        protected ILanguageProvider LanguageProvider { get; }

        protected ICookieService CookieService { get; }

        public LanguageBlazorWasmManager(
            IJSRuntime jsRuntime,
            ILanguageProvider languageProvider,
            ICookieService cookieService)
        {
            JsRuntime = jsRuntime;
            LanguageProvider = languageProvider;
            CookieService = cookieService;
        }

        public virtual async Task ChangeAsync(LanguageInfo newLanguage)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(newLanguage.UiCultureName);

            await JsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                "Abp.SelectedLanguage", newLanguage.UiCultureName
            );

            await JsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                "Abp.IsRtl", CultureHelper.IsRtl
            );

            await CookieService.SetAsync(
                ".AspNetCore.Culture",
                $"c={newLanguage.CultureName}|uic={newLanguage.UiCultureName}",
                new CookieOptions
                {
                    Path = "/"
                }
            );

            await JsRuntime.InvokeVoidAsync("location.reload");
        }

        public virtual async Task<LanguageInfo> GetCurrentAsync()
        {
            var selectedLanguageName = await JsRuntime.InvokeAsync<string>(
                "localStorage.getItem",
                "Abp.SelectedLanguage"
            );

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
    }
}
