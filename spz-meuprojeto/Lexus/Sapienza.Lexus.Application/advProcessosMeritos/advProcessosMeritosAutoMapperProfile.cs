using AutoMapper;
using Sapienza.Lexus.advProcessosMeritos.Dtos;

namespace Sapienza.Lexus.advProcessosMeritos;

public class advProcessosMeritosAutoMapperProfile : Profile
{
    public advProcessosMeritosAutoMapperProfile()
    {
        CreateMap<advProcessosMeritos, advProcessosMeritosDto>();
        CreateMap<CreateUpdateadvProcessosMeritosDto, advProcessosMeritos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProcessosMeritosDto, advProcessosMeritos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
