using AutoMapper;
using Sapienza.Lexus.advPostosINSS.Dtos;

namespace Sapienza.Lexus.advPostosINSS;

public class advPostosINSSAutoMapperProfile : Profile
{
    public advPostosINSSAutoMapperProfile()
    {
        CreateMap<advPostosINSS, advPostosINSSDto>()
            .ForMember(dest => dest.advClientesDisplayName, opt => opt.MapFrom(src => src.advPostosINSSNav.apelido));
        CreateMap<CreateUpdateadvPostosINSSDto, advPostosINSS>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPostosINSSDto, advPostosINSS>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
