using AutoMapper;
using Sapienza.Lexus.advCliSituacoes.Dtos;

namespace Sapienza.Lexus.advCliSituacoes;

public class advCliSituacoesAutoMapperProfile : Profile
{
    public advCliSituacoesAutoMapperProfile()
    {
        CreateMap<advCliSituacoes, advCliSituacoesDto>()
            .ForMember(dest => dest.advClientesDisplayName, opt => opt.MapFrom(src => src.advCliSituacoesNav.apelido));
        CreateMap<CreateUpdateadvCliSituacoesDto, advCliSituacoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliSituacoesDto, advCliSituacoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
