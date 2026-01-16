using AutoMapper;
using Sapienza.Lexus.logCampos.Dtos;

namespace Sapienza.Lexus.logCampos;

public class logCamposAutoMapperProfile : Profile
{
    public logCamposAutoMapperProfile()
    {
        CreateMap<logCampos, logCamposDto>();
        CreateMap<CreateUpdatelogCamposDto, logCampos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatelogCamposDto, logCampos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
