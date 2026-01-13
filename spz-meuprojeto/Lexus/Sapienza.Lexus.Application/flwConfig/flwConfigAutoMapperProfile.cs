using AutoMapper;
using Sapienza.Lexus.flwConfig.Dtos;

namespace Sapienza.Lexus.flwConfig;

public class flwConfigAutoMapperProfile : Profile
{
    public flwConfigAutoMapperProfile()
    {
        CreateMap<flwConfig, flwConfigDto>();
        CreateMap<CreateUpdateflwConfigDto, flwConfig>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateflwConfigDto, flwConfig>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
