using AutoMapper;
using Sapienza.Lexus.fabPaises.Dtos;

namespace Sapienza.Lexus.fabPaises;

public class fabPaisesAutoMapperProfile : Profile
{
    public fabPaisesAutoMapperProfile()
    {
        CreateMap<fabPaises, fabPaisesDto>()
            .ForMember(dest => dest.fabEstadosDisplayName, opt => opt.MapFrom(src => src.fabPaisesNav.sigla));
        CreateMap<CreateUpdatefabPaisesDto, fabPaises>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabPaisesDto, fabPaises>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
