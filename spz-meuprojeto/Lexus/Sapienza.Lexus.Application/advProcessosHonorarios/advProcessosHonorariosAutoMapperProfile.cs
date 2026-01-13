using AutoMapper;
using Sapienza.Lexus.advProcessosHonorarios.Dtos;

namespace Sapienza.Lexus.advProcessosHonorarios;

public class advProcessosHonorariosAutoMapperProfile : Profile
{
    public advProcessosHonorariosAutoMapperProfile()
    {
        CreateMap<advProcessosHonorarios, advProcessosHonorariosDto>();
        CreateMap<CreateUpdateadvProcessosHonorariosDto, advProcessosHonorarios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProcessosHonorariosDto, advProcessosHonorarios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
