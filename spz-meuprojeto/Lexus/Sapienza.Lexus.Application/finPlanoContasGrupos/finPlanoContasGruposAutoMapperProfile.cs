using AutoMapper;
using Sapienza.Lexus.finPlanoContasGrupos.Dtos;

namespace Sapienza.Lexus.finPlanoContasGrupos;

public class finPlanoContasGruposAutoMapperProfile : Profile
{
    public finPlanoContasGruposAutoMapperProfile()
    {
        CreateMap<finPlanoContasGrupos, finPlanoContasGruposDto>()
            .ForMember(dest => dest.finPlanoContasDisplayName, opt => opt.MapFrom(src => src.finPlanoContasGruposNav.titulo));
        CreateMap<CreateUpdatefinPlanoContasGruposDto, finPlanoContasGrupos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinPlanoContasGruposDto, finPlanoContasGrupos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
