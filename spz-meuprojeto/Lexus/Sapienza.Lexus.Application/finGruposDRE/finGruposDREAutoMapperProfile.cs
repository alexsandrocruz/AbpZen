using AutoMapper;
using Sapienza.Lexus.finGruposDRE.Dtos;

namespace Sapienza.Lexus.finGruposDRE;

public class finGruposDREAutoMapperProfile : Profile
{
    public finGruposDREAutoMapperProfile()
    {
        CreateMap<finGruposDRE, finGruposDREDto>();
        CreateMap<CreateUpdatefinGruposDREDto, finGruposDRE>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinGruposDREDto, finGruposDRE>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
