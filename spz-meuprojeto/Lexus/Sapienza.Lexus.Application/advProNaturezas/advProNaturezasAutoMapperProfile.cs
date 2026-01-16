using AutoMapper;
using Sapienza.Lexus.advProNaturezas.Dtos;

namespace Sapienza.Lexus.advProNaturezas;

public class advProNaturezasAutoMapperProfile : Profile
{
    public advProNaturezasAutoMapperProfile()
    {
        CreateMap<advProNaturezas, advProNaturezasDto>()
            .ForMember(dest => dest.advProfissionaisNaturezasDisplayName, opt => opt.MapFrom(src => src.advProNaturezasNav.Id));
        CreateMap<CreateUpdateadvProNaturezasDto, advProNaturezas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProNaturezasDto, advProNaturezas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
