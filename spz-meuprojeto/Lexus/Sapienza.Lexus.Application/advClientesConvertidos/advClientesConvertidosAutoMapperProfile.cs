using AutoMapper;
using Sapienza.Lexus.advClientesConvertidos.Dtos;

namespace Sapienza.Lexus.advClientesConvertidos;

public class advClientesConvertidosAutoMapperProfile : Profile
{
    public advClientesConvertidosAutoMapperProfile()
    {
        CreateMap<advClientesConvertidos, advClientesConvertidosDto>();
        CreateMap<CreateUpdateadvClientesConvertidosDto, advClientesConvertidos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesConvertidosDto, advClientesConvertidos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
