using AutoMapper;
using Sapienza.Lexus.advCliComoChegou.Dtos;

namespace Sapienza.Lexus.advCliComoChegou;

public class advCliComoChegouAutoMapperProfile : Profile
{
    public advCliComoChegouAutoMapperProfile()
    {
        CreateMap<advCliComoChegou, advCliComoChegouDto>();
        CreateMap<CreateUpdateadvCliComoChegouDto, advCliComoChegou>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliComoChegouDto, advCliComoChegou>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
