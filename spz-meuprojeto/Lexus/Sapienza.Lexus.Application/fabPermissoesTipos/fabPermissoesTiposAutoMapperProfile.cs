using AutoMapper;
using Sapienza.Lexus.fabPermissoesTipos.Dtos;

namespace Sapienza.Lexus.fabPermissoesTipos;

public class fabPermissoesTiposAutoMapperProfile : Profile
{
    public fabPermissoesTiposAutoMapperProfile()
    {
        CreateMap<fabPermissoesTipos, fabPermissoesTiposDto>();
        CreateMap<CreateUpdatefabPermissoesTiposDto, fabPermissoesTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabPermissoesTiposDto, fabPermissoesTipos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
