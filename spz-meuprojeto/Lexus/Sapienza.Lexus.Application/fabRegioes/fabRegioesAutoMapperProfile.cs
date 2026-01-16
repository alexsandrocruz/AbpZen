using AutoMapper;
using Sapienza.Lexus.fabRegioes.Dtos;

namespace Sapienza.Lexus.fabRegioes;

public class fabRegioesAutoMapperProfile : Profile
{
    public fabRegioesAutoMapperProfile()
    {
        CreateMap<fabRegioes, fabRegioesDto>();
        CreateMap<CreateUpdatefabRegioesDto, fabRegioes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabRegioesDto, fabRegioes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
