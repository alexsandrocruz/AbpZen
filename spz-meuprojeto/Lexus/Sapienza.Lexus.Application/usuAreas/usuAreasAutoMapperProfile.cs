using AutoMapper;
using Sapienza.Lexus.usuAreas.Dtos;

namespace Sapienza.Lexus.usuAreas;

public class usuAreasAutoMapperProfile : Profile
{
    public usuAreasAutoMapperProfile()
    {
        CreateMap<usuAreas, usuAreasDto>()
            .ForMember(dest => dest.usuCargosDisplayName, opt => opt.MapFrom(src => src.usuAreasNav.titulo));
        CreateMap<CreateUpdateusuAreasDto, usuAreas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateusuAreasDto, usuAreas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
