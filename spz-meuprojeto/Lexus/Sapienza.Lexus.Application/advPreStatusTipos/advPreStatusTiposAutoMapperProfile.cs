using AutoMapper;
using Sapienza.Lexus.advPreStatusTipos.Dtos;

namespace Sapienza.Lexus.advPreStatusTipos;

public class advPreStatusTiposAutoMapperProfile : Profile
{
    public advPreStatusTiposAutoMapperProfile()
    {
        CreateMap<advPreStatusTipos, advPreStatusTiposDto>()
            .ForMember(dest => dest.advPreStatusDisplayName, opt => opt.MapFrom(src => src.advPreStatusTiposNav.titulo));
        CreateMap<CreateUpdateadvPreStatusTiposDto, advPreStatusTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreStatusTiposDto, advPreStatusTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
