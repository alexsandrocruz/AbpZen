using AutoMapper;
using Sapienza.Lexus.opoSituacoes.Dtos;

namespace Sapienza.Lexus.opoSituacoes;

public class opoSituacoesAutoMapperProfile : Profile
{
    public opoSituacoesAutoMapperProfile()
    {
        CreateMap<opoSituacoes, opoSituacoesDto>();
        CreateMap<CreateUpdateopoSituacoesDto, opoSituacoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateopoSituacoesDto, opoSituacoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
