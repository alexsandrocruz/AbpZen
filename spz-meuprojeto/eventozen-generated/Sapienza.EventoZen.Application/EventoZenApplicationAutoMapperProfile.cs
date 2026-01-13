using AutoMapper;

namespace Sapienza.EventoZen
{
    public class EventoZenApplicationAutoMapperProfile : Profile
    {
        public EventoZenApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

                        CreateMap<Sapienza.EventoZen.Artist.Artist, Sapienza.EventoZen.Artist.Dtos.ArtistDto>();
            CreateMap<Sapienza.EventoZen.Artist.Dtos.CreateUpdateArtistDto, Sapienza.EventoZen.Artist.Artist>();
                  CreateMap<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty, Sapienza.EventoZen.ArtistSpecialty.Dtos.ArtistSpecialtyDto>();
            CreateMap<Sapienza.EventoZen.ArtistSpecialty.Dtos.CreateUpdateArtistSpecialtyDto, Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty>();
                  CreateMap<Sapienza.EventoZen.Availability.Availability, Sapienza.EventoZen.Availability.Dtos.AvailabilityDto>();
            CreateMap<Sapienza.EventoZen.Availability.Dtos.CreateUpdateAvailabilityDto, Sapienza.EventoZen.Availability.Availability>();
                  CreateMap<Sapienza.EventoZen.Client.Client, Sapienza.EventoZen.Client.Dtos.ClientDto>();
            CreateMap<Sapienza.EventoZen.Client.Dtos.CreateUpdateClientDto, Sapienza.EventoZen.Client.Client>();
                  CreateMap<Sapienza.EventoZen.Event.Event, Sapienza.EventoZen.Event.Dtos.EventDto>();
            CreateMap<Sapienza.EventoZen.Event.Dtos.CreateUpdateEventDto, Sapienza.EventoZen.Event.Event>();
                  CreateMap<Sapienza.EventoZen.Location.Location, Sapienza.EventoZen.Location.Dtos.LocationDto>();
            CreateMap<Sapienza.EventoZen.Location.Dtos.CreateUpdateLocationDto, Sapienza.EventoZen.Location.Location>();
                  CreateMap<Sapienza.EventoZen.EventCommission.EventCommission, Sapienza.EventoZen.EventCommission.Dtos.EventCommissionDto>();
            CreateMap<Sapienza.EventoZen.EventCommission.Dtos.CreateUpdateEventCommissionDto, Sapienza.EventoZen.EventCommission.EventCommission>();
      // <<GEN-MAPPINGS>>
        }
    }
}
