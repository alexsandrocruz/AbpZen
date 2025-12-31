using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.Abp.LanguageManagement;

public class LanguageManager : DomainService
{
    protected ILanguageRepository LanguageRepository { get; }

    public LanguageManager(ILanguageRepository languageRepository)
    {
        LanguageRepository = languageRepository;
    }

    public virtual async Task<Language> CreateAsync(
        string cultureName,
        string uiCultureName = null,
        string displayName = null,
        bool isEnabled = true)
    {
        if (await LanguageRepository.AnyAsync(cultureName))
        {
            throw new BusinessException(LanguageManagementDomainErrorCodes.CultureNameAlreadyExists)
                .WithData("CultureName", cultureName);
        }

        return new Language(
            GuidGenerator.Create(),
            cultureName,
            uiCultureName,
            displayName,
            isEnabled
        );
    }

}