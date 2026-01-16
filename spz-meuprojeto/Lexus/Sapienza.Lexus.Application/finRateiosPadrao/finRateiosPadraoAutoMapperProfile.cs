using AutoMapper;
using Sapienza.Lexus.finRateiosPadrao.Dtos;

namespace Sapienza.Lexus.finRateiosPadrao;

public class finRateiosPadraoAutoMapperProfile : Profile
{
    public finRateiosPadraoAutoMapperProfile()
    {
        CreateMap<finRateiosPadrao, finRateiosPadraoDto>();
        CreateMap<CreateUpdatefinRateiosPadraoDto, finRateiosPadrao>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinRateiosPadraoDto, finRateiosPadrao>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
