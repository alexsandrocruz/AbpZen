using AutoMapper;
using Sapienza.Lexus.finCentrosCusto.Dtos;

namespace Sapienza.Lexus.finCentrosCusto;

public class finCentrosCustoAutoMapperProfile : Profile
{
    public finCentrosCustoAutoMapperProfile()
    {
        CreateMap<finCentrosCusto, finCentrosCustoDto>()
            .ForMember(dest => dest.finLancamentosDisplayName, opt => opt.MapFrom(src => src.finCentrosCustoNav.operacao));
        CreateMap<CreateUpdatefinCentrosCustoDto, finCentrosCusto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinCentrosCustoDto, finCentrosCusto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
