using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;

namespace Volo.Abp.LanguageManagement;

public class LanguageManagementDomainAutoMapperProfile : Profile
{
    public LanguageManagementDomainAutoMapperProfile()
    {
        CreateMap<Language, LanguageInfo>()
            .Ignore(x => x.TwoLetterISOLanguageName);
        CreateMap<Language, LanguageEto>();
        CreateMap<LanguageText, LanguageTextEto>();
    }
}
