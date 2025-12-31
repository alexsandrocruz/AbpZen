using Volo.Abp.DependencyInjection;
using DependencyAttribute = Volo.Abp.DependencyInjection.DependencyAttribute;
namespace Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme;

//TODO : When ABP 7.0-RC.3 is released
// [Dependency(ReplaceServices = true)]
// public class MauiBlazorSelectedLanguageProvider : IMauiBlazorSelectedLanguageProvider, ITransientDependency
// {
//     public Task<string> GetSelectedLanguageAsync()
//     {
//         return Task.FromResult(Preferences.Get("Abp.SelectedLanguage", string.Empty));
//     }
// }