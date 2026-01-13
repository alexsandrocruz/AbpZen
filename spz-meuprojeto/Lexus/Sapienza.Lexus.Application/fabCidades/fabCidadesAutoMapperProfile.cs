using AutoMapper;
using Sapienza.Lexus.fabCidades.Dtos;

namespace Sapienza.Lexus.fabCidades;

public class fabCidadesAutoMapperProfile : Profile
{
    public fabCidadesAutoMapperProfile()
    {
        CreateMap<fabCidades, fabCidadesDto>();
        CreateMap<CreateUpdatefabCidadesDto, fabCidades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabCidadesDto, fabCidades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
