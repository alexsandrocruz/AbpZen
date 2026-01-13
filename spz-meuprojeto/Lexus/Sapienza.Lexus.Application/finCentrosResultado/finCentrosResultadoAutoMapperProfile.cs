using AutoMapper;
using Sapienza.Lexus.finCentrosResultado.Dtos;

namespace Sapienza.Lexus.finCentrosResultado;

public class finCentrosResultadoAutoMapperProfile : Profile
{
    public finCentrosResultadoAutoMapperProfile()
    {
        CreateMap<finCentrosResultado, finCentrosResultadoDto>();
        CreateMap<CreateUpdatefinCentrosResultadoDto, finCentrosResultado>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinCentrosResultadoDto, finCentrosResultado>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
