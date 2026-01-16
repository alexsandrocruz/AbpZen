using AutoMapper;
using Sapienza.Lexus.advClientesINSSStatus.Dtos;

namespace Sapienza.Lexus.advClientesINSSStatus;

public class advClientesINSSStatusAutoMapperProfile : Profile
{
    public advClientesINSSStatusAutoMapperProfile()
    {
        CreateMap<advClientesINSSStatus, advClientesINSSStatusDto>()
            .ForMember(dest => dest.advClientesINSSDisplayName, opt => opt.MapFrom(src => src.advClientesINSSStatusNav.inssData));
        CreateMap<CreateUpdateadvClientesINSSStatusDto, advClientesINSSStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesINSSStatusDto, advClientesINSSStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
