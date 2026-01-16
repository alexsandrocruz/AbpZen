using AutoMapper;
using Sapienza.Lexus.flwConfigExcecoes.Dtos;

namespace Sapienza.Lexus.flwConfigExcecoes;

public class flwConfigExcecoesAutoMapperProfile : Profile
{
    public flwConfigExcecoesAutoMapperProfile()
    {
        CreateMap<flwConfigExcecoes, flwConfigExcecoesDto>();
        CreateMap<CreateUpdateflwConfigExcecoesDto, flwConfigExcecoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateflwConfigExcecoesDto, flwConfigExcecoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
