using AutoMapper;
using Sapienza.Lexus.fabEstados.Dtos;

namespace Sapienza.Lexus.fabEstados;

public class fabEstadosAutoMapperProfile : Profile
{
    public fabEstadosAutoMapperProfile()
    {
        CreateMap<fabEstados, fabEstadosDto>()
            .ForMember(dest => dest.fabCidadesDisplayName, opt => opt.MapFrom(src => src.fabEstadosNav.descricao));
        CreateMap<CreateUpdatefabEstadosDto, fabEstados>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabEstadosDto, fabEstados>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
