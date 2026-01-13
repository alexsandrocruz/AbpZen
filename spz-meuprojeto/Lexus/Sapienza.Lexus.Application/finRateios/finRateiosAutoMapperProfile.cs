using AutoMapper;
using Sapienza.Lexus.finRateios.Dtos;

namespace Sapienza.Lexus.finRateios;

public class finRateiosAutoMapperProfile : Profile
{
    public finRateiosAutoMapperProfile()
    {
        CreateMap<finRateios, finRateiosDto>();
        CreateMap<CreateUpdatefinRateiosDto, finRateios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinRateiosDto, finRateios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
