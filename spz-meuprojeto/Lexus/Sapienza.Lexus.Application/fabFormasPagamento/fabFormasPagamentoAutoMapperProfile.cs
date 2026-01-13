using AutoMapper;
using Sapienza.Lexus.fabFormasPagamento.Dtos;

namespace Sapienza.Lexus.fabFormasPagamento;

public class fabFormasPagamentoAutoMapperProfile : Profile
{
    public fabFormasPagamentoAutoMapperProfile()
    {
        CreateMap<fabFormasPagamento, fabFormasPagamentoDto>();
        CreateMap<CreateUpdatefabFormasPagamentoDto, fabFormasPagamento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabFormasPagamentoDto, fabFormasPagamento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
