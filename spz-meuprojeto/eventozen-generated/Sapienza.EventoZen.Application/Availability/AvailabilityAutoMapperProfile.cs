using AutoMapper;
using Sapienza.EventoZen.Availability.Dtos;

namespace Sapienza.EventoZen.Availability;

public class AvailabilityAutoMapperProfile : Profile
{
    public AvailabilityAutoMapperProfile()
    {
        CreateMap<Availability, AvailabilityDto>()
            .ForMember(dest => dest.ArtistDisplayName, opt => opt.MapFrom(src => src.Artist.Name));
        CreateMap<CreateUpdateAvailabilityDto, Availability>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateAvailabilityDto, Availability>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
