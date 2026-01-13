using AutoMapper;
using Sapienza.Lexus.opoOrcamentos.Dtos;

namespace Sapienza.Lexus.opoOrcamentos;

public class opoOrcamentosAutoMapperProfile : Profile
{
    public opoOrcamentosAutoMapperProfile()
    {
        CreateMap<opoOrcamentos, opoOrcamentosDto>();
        CreateMap<CreateUpdateopoOrcamentosDto, opoOrcamentos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateopoOrcamentosDto, opoOrcamentos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
