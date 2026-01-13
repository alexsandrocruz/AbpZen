using AutoMapper;
using Sapienza.Lexus.advProfissionaisNaturezas.Dtos;

namespace Sapienza.Lexus.advProfissionaisNaturezas;

public class advProfissionaisNaturezasAutoMapperProfile : Profile
{
    public advProfissionaisNaturezasAutoMapperProfile()
    {
        CreateMap<advProfissionaisNaturezas, advProfissionaisNaturezasDto>();
        CreateMap<CreateUpdateadvProfissionaisNaturezasDto, advProfissionaisNaturezas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProfissionaisNaturezasDto, advProfissionaisNaturezas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
