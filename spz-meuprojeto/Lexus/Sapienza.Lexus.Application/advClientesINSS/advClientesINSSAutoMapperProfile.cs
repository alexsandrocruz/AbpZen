using AutoMapper;
using Sapienza.Lexus.advClientesINSS.Dtos;

namespace Sapienza.Lexus.advClientesINSS;

public class advClientesINSSAutoMapperProfile : Profile
{
    public advClientesINSSAutoMapperProfile()
    {
        CreateMap<advClientesINSS, advClientesINSSDto>();
        CreateMap<CreateUpdateadvClientesINSSDto, advClientesINSS>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesINSSDto, advClientesINSS>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
