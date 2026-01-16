using AutoMapper;
using Sapienza.Lexus.usuAcessos.Dtos;

namespace Sapienza.Lexus.usuAcessos;

public class usuAcessosAutoMapperProfile : Profile
{
    public usuAcessosAutoMapperProfile()
    {
        CreateMap<usuAcessos, usuAcessosDto>();
        CreateMap<CreateUpdateusuAcessosDto, usuAcessos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateusuAcessosDto, usuAcessos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
