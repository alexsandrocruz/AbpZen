using AutoMapper;
using Sapienza.Lexus.logAcoes.Dtos;

namespace Sapienza.Lexus.logAcoes;

public class logAcoesAutoMapperProfile : Profile
{
    public logAcoesAutoMapperProfile()
    {
        CreateMap<logAcoes, logAcoesDto>()
            .ForMember(dest => dest.logCamposDisplayName, opt => opt.MapFrom(src => src.logAcoesNav.campo));
        CreateMap<CreateUpdatelogAcoesDto, logAcoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatelogAcoesDto, logAcoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
