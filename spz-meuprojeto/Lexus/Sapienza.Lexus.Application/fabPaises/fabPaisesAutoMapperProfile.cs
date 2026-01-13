using AutoMapper;
using Sapienza.Lexus.fabPaises.Dtos;

namespace Sapienza.Lexus.fabPaises;

public class fabPaisesAutoMapperProfile : Profile
{
    public fabPaisesAutoMapperProfile()
    {
        CreateMap<fabPaises, fabPaisesDto>();
        CreateMap<CreateUpdatefabPaisesDto, fabPaises>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabPaisesDto, fabPaises>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
