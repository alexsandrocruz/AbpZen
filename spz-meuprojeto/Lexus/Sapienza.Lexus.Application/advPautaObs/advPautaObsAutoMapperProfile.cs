using AutoMapper;
using Sapienza.Lexus.advPautaObs.Dtos;

namespace Sapienza.Lexus.advPautaObs;

public class advPautaObsAutoMapperProfile : Profile
{
    public advPautaObsAutoMapperProfile()
    {
        CreateMap<advPautaObs, advPautaObsDto>();
        CreateMap<CreateUpdateadvPautaObsDto, advPautaObs>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPautaObsDto, advPautaObs>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
