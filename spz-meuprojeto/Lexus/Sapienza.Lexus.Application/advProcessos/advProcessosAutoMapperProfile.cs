using AutoMapper;
using Sapienza.Lexus.advProcessos.Dtos;

namespace Sapienza.Lexus.advProcessos;

public class advProcessosAutoMapperProfile : Profile
{
    public advProcessosAutoMapperProfile()
    {
        CreateMap<advProcessos, advProcessosDto>();
        CreateMap<CreateUpdateadvProcessosDto, advProcessos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProcessosDto, advProcessos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
