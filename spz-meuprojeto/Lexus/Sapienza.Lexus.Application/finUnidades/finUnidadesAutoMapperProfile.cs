using AutoMapper;
using Sapienza.Lexus.finUnidades.Dtos;

namespace Sapienza.Lexus.finUnidades;

public class finUnidadesAutoMapperProfile : Profile
{
    public finUnidadesAutoMapperProfile()
    {
        CreateMap<finUnidades, finUnidadesDto>();
        CreateMap<CreateUpdatefinUnidadesDto, finUnidades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinUnidadesDto, finUnidades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
