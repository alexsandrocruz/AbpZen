using AutoMapper;
using Sapienza.Lexus.advCliPrioridades.Dtos;

namespace Sapienza.Lexus.advCliPrioridades;

public class advCliPrioridadesAutoMapperProfile : Profile
{
    public advCliPrioridadesAutoMapperProfile()
    {
        CreateMap<advCliPrioridades, advCliPrioridadesDto>();
        CreateMap<CreateUpdateadvCliPrioridadesDto, advCliPrioridades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliPrioridadesDto, advCliPrioridades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
