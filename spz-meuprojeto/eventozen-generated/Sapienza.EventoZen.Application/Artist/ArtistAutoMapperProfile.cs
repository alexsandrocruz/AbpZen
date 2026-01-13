using AutoMapper;
using Sapienza.EventoZen.Artist.Dtos;

namespace Sapienza.EventoZen.Artist;

public class ArtistAutoMapperProfile : Profile
{
    public ArtistAutoMapperProfile()
    {
        CreateMap<Artist, ArtistDto>();
        CreateMap<CreateUpdateArtistDto, Artist>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateArtistDto, Artist>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
