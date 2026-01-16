using AutoMapper;
using Sapienza.Lexus.advProMeritos.Dtos;

namespace Sapienza.Lexus.advProMeritos;

public class advProMeritosAutoMapperProfile : Profile
{
    public advProMeritosAutoMapperProfile()
    {
        CreateMap<advProMeritos, advProMeritosDto>()
            .ForMember(dest => dest.advProcessosMeritosDisplayName, opt => opt.MapFrom(src => src.advProMeritosNav.Id));
        CreateMap<CreateUpdateadvProMeritosDto, advProMeritos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProMeritosDto, advProMeritos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
