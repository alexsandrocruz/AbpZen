using AutoMapper;
using Sapienza.Lexus.usuUsuarios.Dtos;

namespace Sapienza.Lexus.usuUsuarios;

public class usuUsuariosAutoMapperProfile : Profile
{
    public usuUsuariosAutoMapperProfile()
    {
        CreateMap<usuUsuarios, usuUsuariosDto>();
        CreateMap<CreateUpdateusuUsuariosDto, usuUsuarios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateusuUsuariosDto, usuUsuarios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
