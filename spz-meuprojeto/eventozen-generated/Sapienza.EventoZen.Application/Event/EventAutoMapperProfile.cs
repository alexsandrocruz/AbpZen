using AutoMapper;
using Sapienza.EventoZen.Event.Dtos;

namespace Sapienza.EventoZen.Event;

public class EventAutoMapperProfile : Profile
{
    public EventAutoMapperProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.ArtistDisplayName, opt => opt.MapFrom(src => src.Artist.Name))
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.LocationDisplayName, opt => opt.MapFrom(src => src.Location.Name));
        CreateMap<CreateUpdateEventDto, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateEventDto, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
