using AutoMapper;
using Sapienza.Lexus.advCliGrupos.Dtos;

namespace Sapienza.Lexus.advCliGrupos;

public class advCliGruposAutoMapperProfile : Profile
{
    public advCliGruposAutoMapperProfile()
    {
        CreateMap<advCliGrupos, advCliGruposDto>()
            .ForMember(dest => dest.advClientesDisplayName, opt => opt.MapFrom(src => src.advCliGruposNav.apelido));
        CreateMap<CreateUpdateadvCliGruposDto, advCliGrupos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliGruposDto, advCliGrupos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
