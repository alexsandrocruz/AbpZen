using AutoMapper;
using Sapienza.Lexus.advPostosINSS.Dtos;

namespace Sapienza.Lexus.advPostosINSS;

public class advPostosINSSAutoMapperProfile : Profile
{
    public advPostosINSSAutoMapperProfile()
    {
        CreateMap<advPostosINSS, advPostosINSSDto>();
        CreateMap<CreateUpdateadvPostosINSSDto, advPostosINSS>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPostosINSSDto, advPostosINSS>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
