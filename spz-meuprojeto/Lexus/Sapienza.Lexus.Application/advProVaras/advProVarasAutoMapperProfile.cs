using AutoMapper;
using Sapienza.Lexus.advProVaras.Dtos;

namespace Sapienza.Lexus.advProVaras;

public class advProVarasAutoMapperProfile : Profile
{
    public advProVarasAutoMapperProfile()
    {
        CreateMap<advProVaras, advProVarasDto>();
        CreateMap<CreateUpdateadvProVarasDto, advProVaras>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProVarasDto, advProVaras>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
