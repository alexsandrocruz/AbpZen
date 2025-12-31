using System.Threading.Tasks;

namespace Volo.Abp.LanguageManagement.External;

public interface IExternalLocalizationSaver
{
    Task SaveAsync();
}