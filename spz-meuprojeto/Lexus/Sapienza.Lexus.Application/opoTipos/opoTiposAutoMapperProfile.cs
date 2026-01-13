using AutoMapper;
using Sapienza.Lexus.opoTipos.Dtos;

namespace Sapienza.Lexus.opoTipos;

public class opoTiposAutoMapperProfile : Profile
{
    public opoTiposAutoMapperProfile()
    {
        CreateMap<opoTipos, opoTiposDto>();
        CreateMap<CreateUpdateopoTiposDto, opoTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateopoTiposDto, opoTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
