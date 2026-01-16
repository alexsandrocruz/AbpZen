using AutoMapper;
using Sapienza.Lexus.advProInstancias.Dtos;

namespace Sapienza.Lexus.advProInstancias;

public class advProInstanciasAutoMapperProfile : Profile
{
    public advProInstanciasAutoMapperProfile()
    {
        CreateMap<advProInstancias, advProInstanciasDto>();
        CreateMap<CreateUpdateadvProInstanciasDto, advProInstancias>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProInstanciasDto, advProInstancias>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
