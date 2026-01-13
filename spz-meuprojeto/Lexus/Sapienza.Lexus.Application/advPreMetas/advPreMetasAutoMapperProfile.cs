using AutoMapper;
using Sapienza.Lexus.advPreMetas.Dtos;

namespace Sapienza.Lexus.advPreMetas;

public class advPreMetasAutoMapperProfile : Profile
{
    public advPreMetasAutoMapperProfile()
    {
        CreateMap<advPreMetas, advPreMetasDto>();
        CreateMap<CreateUpdateadvPreMetasDto, advPreMetas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreMetasDto, advPreMetas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
