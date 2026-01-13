using AutoMapper;
using Sapienza.Lexus.fabEstados.Dtos;

namespace Sapienza.Lexus.fabEstados;

public class fabEstadosAutoMapperProfile : Profile
{
    public fabEstadosAutoMapperProfile()
    {
        CreateMap<fabEstados, fabEstadosDto>();
        CreateMap<CreateUpdatefabEstadosDto, fabEstados>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabEstadosDto, fabEstados>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
