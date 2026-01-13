using AutoMapper;
using Sapienza.Lexus.fabConfig.Dtos;

namespace Sapienza.Lexus.fabConfig;

public class fabConfigAutoMapperProfile : Profile
{
    public fabConfigAutoMapperProfile()
    {
        CreateMap<fabConfig, fabConfigDto>();
        CreateMap<CreateUpdatefabConfigDto, fabConfig>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabConfigDto, fabConfig>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
