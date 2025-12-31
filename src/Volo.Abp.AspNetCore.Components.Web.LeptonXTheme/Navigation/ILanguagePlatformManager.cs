using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation
{
    public interface ILanguagePlatformManager
    {
        Task ChangeAsync(LanguageInfo newLanguage);

        Task<LanguageInfo> GetCurrentAsync();
    }
}
