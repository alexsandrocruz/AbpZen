using AutoMapper;
using Sapienza.Lexus.finContas.Dtos;

namespace Sapienza.Lexus.finContas;

public class finContasAutoMapperProfile : Profile
{
    public finContasAutoMapperProfile()
    {
        CreateMap<finContas, finContasDto>()
            .ForMember(dest => dest.finExtratoDisplayName, opt => opt.MapFrom(src => src.finContasNav.data))
            .ForMember(dest => dest.finLancamentosDisplayName, opt => opt.MapFrom(src => src.finContasNav1.operacao));
        CreateMap<CreateUpdatefinContasDto, finContas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinContasDto, finContas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
