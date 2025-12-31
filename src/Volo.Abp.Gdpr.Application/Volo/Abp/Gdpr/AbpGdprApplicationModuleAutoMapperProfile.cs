using AutoMapper;

namespace Volo.Abp.Gdpr;

public class AbpGdprApplicationModuleAutoMapperProfile : Profile
{
    public AbpGdprApplicationModuleAutoMapperProfile()
    {
        CreateMap<GdprRequest, GdprRequestDto>();
    }
}