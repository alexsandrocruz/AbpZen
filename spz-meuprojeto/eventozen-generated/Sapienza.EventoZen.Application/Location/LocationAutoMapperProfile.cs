using AutoMapper;
using Sapienza.EventoZen.Location.Dtos;

namespace Sapienza.EventoZen.Location;

public class LocationAutoMapperProfile : Profile
{
    public LocationAutoMapperProfile()
    {
        CreateMap<Location, LocationDto>();
        CreateMap<CreateUpdateLocationDto, Location>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLocationDto, Location>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
