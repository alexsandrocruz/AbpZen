using AutoMapper;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;

namespace Sapienza.Lexus.fabHistoricoTipos;

public class fabHistoricoTiposAutoMapperProfile : Profile
{
    public fabHistoricoTiposAutoMapperProfile()
    {
        CreateMap<fabHistoricoTipos, fabHistoricoTiposDto>();
        CreateMap<CreateUpdatefabHistoricoTiposDto, fabHistoricoTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabHistoricoTiposDto, fabHistoricoTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
