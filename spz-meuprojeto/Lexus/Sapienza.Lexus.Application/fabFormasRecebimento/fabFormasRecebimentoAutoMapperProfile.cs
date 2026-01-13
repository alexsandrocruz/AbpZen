using AutoMapper;
using Sapienza.Lexus.fabFormasRecebimento.Dtos;

namespace Sapienza.Lexus.fabFormasRecebimento;

public class fabFormasRecebimentoAutoMapperProfile : Profile
{
    public fabFormasRecebimentoAutoMapperProfile()
    {
        CreateMap<fabFormasRecebimento, fabFormasRecebimentoDto>();
        CreateMap<CreateUpdatefabFormasRecebimentoDto, fabFormasRecebimento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabFormasRecebimentoDto, fabFormasRecebimento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
