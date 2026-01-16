using AutoMapper;
using Sapienza.Lexus.advCliBairros.Dtos;

namespace Sapienza.Lexus.advCliBairros;

public class advCliBairrosAutoMapperProfile : Profile
{
    public advCliBairrosAutoMapperProfile()
    {
        CreateMap<advCliBairros, advCliBairrosDto>();
        CreateMap<CreateUpdateadvCliBairrosDto, advCliBairros>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliBairrosDto, advCliBairros>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
