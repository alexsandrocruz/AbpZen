using AutoMapper;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;

namespace Sapienza.Lexus.fabHistoricoTipos;

public class fabHistoricoTiposAutoMapperProfile : Profile
{
    public fabHistoricoTiposAutoMapperProfile()
    {
        CreateMap<fabHistoricoTipos, fabHistoricoTiposDto>()
            .ForMember(dest => dest.flwConfigExcecoesDisplayName, opt => opt.MapFrom(src => src.fabHistoricoTiposNav.tipoMarcacoes))
            .ForMember(dest => dest.flwGradeHorariosDisplayName, opt => opt.MapFrom(src => src.fabHistoricoTiposNav1.Id))
            .ForMember(dest => dest.flwFollowsDisplayName, opt => opt.MapFrom(src => src.fabHistoricoTiposNav2.data));
        CreateMap<CreateUpdatefabHistoricoTiposDto, fabHistoricoTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabHistoricoTiposDto, fabHistoricoTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
