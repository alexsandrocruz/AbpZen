using AutoMapper;
using Sapienza.Lexus.advProfissionais.Dtos;

namespace Sapienza.Lexus.advProfissionais;

public class advProfissionaisAutoMapperProfile : Profile
{
    public advProfissionaisAutoMapperProfile()
    {
        CreateMap<advProfissionais, advProfissionaisDto>()
            .ForMember(dest => dest.advProfissionaisEstadosDisplayName, opt => opt.MapFrom(src => src.advProfissionaisNav.estado))
            .ForMember(dest => dest.advProfissionaisNaturezasDisplayName, opt => opt.MapFrom(src => src.advProfissionaisNav1.Id))
            .ForMember(dest => dest.advVerbasDisplayName, opt => opt.MapFrom(src => src.advProfissionaisNav2.dataDe));
        CreateMap<CreateUpdateadvProfissionaisDto, advProfissionais>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProfissionaisDto, advProfissionais>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
