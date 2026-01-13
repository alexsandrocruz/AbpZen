using AutoMapper;
using Sapienza.EventoZen.ArtistSpecialty.Dtos;

namespace Sapienza.EventoZen.ArtistSpecialty;

public class ArtistSpecialtyAutoMapperProfile : Profile
{
    public ArtistSpecialtyAutoMapperProfile()
    {
        CreateMap<ArtistSpecialty, ArtistSpecialtyDto>()
            .ForMember(dest => dest.ArtistDisplayName, opt => opt.MapFrom(src => src.Artist.Name));
        CreateMap<CreateUpdateArtistSpecialtyDto, ArtistSpecialty>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateArtistSpecialtyDto, ArtistSpecialty>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
