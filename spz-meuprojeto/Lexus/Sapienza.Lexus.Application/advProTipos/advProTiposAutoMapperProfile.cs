using AutoMapper;
using Sapienza.Lexus.advProTipos.Dtos;

namespace Sapienza.Lexus.advProTipos;

public class advProTiposAutoMapperProfile : Profile
{
    public advProTiposAutoMapperProfile()
    {
        CreateMap<advProTipos, advProTiposDto>();
        CreateMap<CreateUpdateadvProTiposDto, advProTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProTiposDto, advProTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
