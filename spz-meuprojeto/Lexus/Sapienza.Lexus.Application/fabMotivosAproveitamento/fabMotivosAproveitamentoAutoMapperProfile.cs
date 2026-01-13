using AutoMapper;
using Sapienza.Lexus.fabMotivosAproveitamento.Dtos;

namespace Sapienza.Lexus.fabMotivosAproveitamento;

public class fabMotivosAproveitamentoAutoMapperProfile : Profile
{
    public fabMotivosAproveitamentoAutoMapperProfile()
    {
        CreateMap<fabMotivosAproveitamento, fabMotivosAproveitamentoDto>();
        CreateMap<CreateUpdatefabMotivosAproveitamentoDto, fabMotivosAproveitamento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabMotivosAproveitamentoDto, fabMotivosAproveitamento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
