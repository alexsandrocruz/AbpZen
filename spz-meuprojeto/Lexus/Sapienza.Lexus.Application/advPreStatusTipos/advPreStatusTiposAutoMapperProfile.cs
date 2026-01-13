using AutoMapper;
using Sapienza.Lexus.advPreStatusTipos.Dtos;

namespace Sapienza.Lexus.advPreStatusTipos;

public class advPreStatusTiposAutoMapperProfile : Profile
{
    public advPreStatusTiposAutoMapperProfile()
    {
        CreateMap<advPreStatusTipos, advPreStatusTiposDto>();
        CreateMap<CreateUpdateadvPreStatusTiposDto, advPreStatusTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreStatusTiposDto, advPreStatusTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
