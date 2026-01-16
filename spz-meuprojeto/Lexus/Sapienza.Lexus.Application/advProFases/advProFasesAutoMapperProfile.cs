using AutoMapper;
using Sapienza.Lexus.advProFases.Dtos;

namespace Sapienza.Lexus.advProFases;

public class advProFasesAutoMapperProfile : Profile
{
    public advProFasesAutoMapperProfile()
    {
        CreateMap<advProFases, advProFasesDto>();
        CreateMap<CreateUpdateadvProFasesDto, advProFases>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProFasesDto, advProFases>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
