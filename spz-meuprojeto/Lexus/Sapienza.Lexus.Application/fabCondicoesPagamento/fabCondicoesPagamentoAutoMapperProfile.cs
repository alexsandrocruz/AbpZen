using AutoMapper;
using Sapienza.Lexus.fabCondicoesPagamento.Dtos;

namespace Sapienza.Lexus.fabCondicoesPagamento;

public class fabCondicoesPagamentoAutoMapperProfile : Profile
{
    public fabCondicoesPagamentoAutoMapperProfile()
    {
        CreateMap<fabCondicoesPagamento, fabCondicoesPagamentoDto>();
        CreateMap<CreateUpdatefabCondicoesPagamentoDto, fabCondicoesPagamento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabCondicoesPagamentoDto, fabCondicoesPagamento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
