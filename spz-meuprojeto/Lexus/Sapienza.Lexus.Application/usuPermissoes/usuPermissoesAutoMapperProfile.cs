using AutoMapper;
using Sapienza.Lexus.usuPermissoes.Dtos;

namespace Sapienza.Lexus.usuPermissoes;

public class usuPermissoesAutoMapperProfile : Profile
{
    public usuPermissoesAutoMapperProfile()
    {
        CreateMap<usuPermissoes, usuPermissoesDto>();
        CreateMap<CreateUpdateusuPermissoesDto, usuPermissoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateusuPermissoesDto, usuPermissoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
