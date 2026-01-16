using AutoMapper;
using Sapienza.Lexus.advAgeTiposCompromissos.Dtos;

namespace Sapienza.Lexus.advAgeTiposCompromissos;

public class advAgeTiposCompromissosAutoMapperProfile : Profile
{
    public advAgeTiposCompromissosAutoMapperProfile()
    {
        CreateMap<advAgeTiposCompromissos, advAgeTiposCompromissosDto>()
            .ForMember(dest => dest.advCompromissosDisplayName, opt => opt.MapFrom(src => src.advAgeTiposCompromissosNav.dataPublicacao));
        CreateMap<CreateUpdateadvAgeTiposCompromissosDto, advAgeTiposCompromissos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvAgeTiposCompromissosDto, advAgeTiposCompromissos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
