using AutoMapper;
using Sapienza.Lexus.advPreCheckListsGrupos.Dtos;

namespace Sapienza.Lexus.advPreCheckListsGrupos;

public class advPreCheckListsGruposAutoMapperProfile : Profile
{
    public advPreCheckListsGruposAutoMapperProfile()
    {
        CreateMap<advPreCheckListsGrupos, advPreCheckListsGruposDto>()
            .ForMember(dest => dest.advPreCheckListsDisplayName, opt => opt.MapFrom(src => src.advPreCheckListsGruposNav.titulo));
        CreateMap<CreateUpdateadvPreCheckListsGruposDto, advPreCheckListsGrupos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreCheckListsGruposDto, advPreCheckListsGrupos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
