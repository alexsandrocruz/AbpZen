using AutoMapper;
using Sapienza.Lexus.advVerbas.Dtos;

namespace Sapienza.Lexus.advVerbas;

public class advVerbasAutoMapperProfile : Profile
{
    public advVerbasAutoMapperProfile()
    {
        CreateMap<advVerbas, advVerbasDto>();
        CreateMap<CreateUpdateadvVerbasDto, advVerbas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvVerbasDto, advVerbas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
