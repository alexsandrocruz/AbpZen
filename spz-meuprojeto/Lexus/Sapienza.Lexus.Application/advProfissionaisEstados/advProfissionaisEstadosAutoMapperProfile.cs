using AutoMapper;
using Sapienza.Lexus.advProfissionaisEstados.Dtos;

namespace Sapienza.Lexus.advProfissionaisEstados;

public class advProfissionaisEstadosAutoMapperProfile : Profile
{
    public advProfissionaisEstadosAutoMapperProfile()
    {
        CreateMap<advProfissionaisEstados, advProfissionaisEstadosDto>();
        CreateMap<CreateUpdateadvProfissionaisEstadosDto, advProfissionaisEstados>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProfissionaisEstadosDto, advProfissionaisEstados>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
