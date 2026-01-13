using AutoMapper;
using Sapienza.Lexus.flwAcoes.Dtos;

namespace Sapienza.Lexus.flwAcoes;

public class flwAcoesAutoMapperProfile : Profile
{
    public flwAcoesAutoMapperProfile()
    {
        CreateMap<flwAcoes, flwAcoesDto>();
        CreateMap<CreateUpdateflwAcoesDto, flwAcoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateflwAcoesDto, flwAcoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
