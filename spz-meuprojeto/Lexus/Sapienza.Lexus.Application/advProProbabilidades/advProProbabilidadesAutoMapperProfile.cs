using AutoMapper;
using Sapienza.Lexus.advProProbabilidades.Dtos;

namespace Sapienza.Lexus.advProProbabilidades;

public class advProProbabilidadesAutoMapperProfile : Profile
{
    public advProProbabilidadesAutoMapperProfile()
    {
        CreateMap<advProProbabilidades, advProProbabilidadesDto>();
        CreateMap<CreateUpdateadvProProbabilidadesDto, advProProbabilidades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProProbabilidadesDto, advProProbabilidades>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
