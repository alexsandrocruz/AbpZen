using AutoMapper;
using Sapienza.Lexus.opoOportunidades.Dtos;

namespace Sapienza.Lexus.opoOportunidades;

public class opoOportunidadesAutoMapperProfile : Profile
{
    public opoOportunidadesAutoMapperProfile()
    {
        CreateMap<opoOportunidades, opoOportunidadesDto>();
        CreateMap<CreateUpdateopoOportunidadesDto, opoOportunidades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateopoOportunidadesDto, opoOportunidades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
