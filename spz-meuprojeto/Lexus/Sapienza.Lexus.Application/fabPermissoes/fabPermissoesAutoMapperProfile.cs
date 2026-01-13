using AutoMapper;
using Sapienza.Lexus.fabPermissoes.Dtos;

namespace Sapienza.Lexus.fabPermissoes;

public class fabPermissoesAutoMapperProfile : Profile
{
    public fabPermissoesAutoMapperProfile()
    {
        CreateMap<fabPermissoes, fabPermissoesDto>();
        CreateMap<CreateUpdatefabPermissoesDto, fabPermissoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabPermissoesDto, fabPermissoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
