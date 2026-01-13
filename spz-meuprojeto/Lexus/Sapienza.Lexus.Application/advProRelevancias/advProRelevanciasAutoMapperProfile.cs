using AutoMapper;
using Sapienza.Lexus.advProRelevancias.Dtos;

namespace Sapienza.Lexus.advProRelevancias;

public class advProRelevanciasAutoMapperProfile : Profile
{
    public advProRelevanciasAutoMapperProfile()
    {
        CreateMap<advProRelevancias, advProRelevanciasDto>();
        CreateMap<CreateUpdateadvProRelevanciasDto, advProRelevancias>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProRelevanciasDto, advProRelevancias>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
