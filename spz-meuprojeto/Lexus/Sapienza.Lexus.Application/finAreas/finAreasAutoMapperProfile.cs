using AutoMapper;
using Sapienza.Lexus.finAreas.Dtos;

namespace Sapienza.Lexus.finAreas;

public class finAreasAutoMapperProfile : Profile
{
    public finAreasAutoMapperProfile()
    {
        CreateMap<finAreas, finAreasDto>()
            .ForMember(dest => dest.finLancamentosDisplayName, opt => opt.MapFrom(src => src.finAreasNav.operacao));
        CreateMap<CreateUpdatefinAreasDto, finAreas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinAreasDto, finAreas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
