using AutoMapper;
using Sapienza.Lexus.advVerTipos.Dtos;

namespace Sapienza.Lexus.advVerTipos;

public class advVerTiposAutoMapperProfile : Profile
{
    public advVerTiposAutoMapperProfile()
    {
        CreateMap<advVerTipos, advVerTiposDto>();
        CreateMap<CreateUpdateadvVerTiposDto, advVerTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvVerTiposDto, advVerTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
