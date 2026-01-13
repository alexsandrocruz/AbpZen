using AutoMapper;
using Sapienza.Lexus.advProEscritorios.Dtos;

namespace Sapienza.Lexus.advProEscritorios;

public class advProEscritoriosAutoMapperProfile : Profile
{
    public advProEscritoriosAutoMapperProfile()
    {
        CreateMap<advProEscritorios, advProEscritoriosDto>();
        CreateMap<CreateUpdateadvProEscritoriosDto, advProEscritorios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProEscritoriosDto, advProEscritorios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
