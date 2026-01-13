using AutoMapper;
using Sapienza.Lexus.usuDistancias.Dtos;

namespace Sapienza.Lexus.usuDistancias;

public class usuDistanciasAutoMapperProfile : Profile
{
    public usuDistanciasAutoMapperProfile()
    {
        CreateMap<usuDistancias, usuDistanciasDto>();
        CreateMap<CreateUpdateusuDistanciasDto, usuDistancias>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateusuDistanciasDto, usuDistancias>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
