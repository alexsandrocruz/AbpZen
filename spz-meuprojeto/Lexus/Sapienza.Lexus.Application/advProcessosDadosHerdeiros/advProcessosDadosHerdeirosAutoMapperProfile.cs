using AutoMapper;
using Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;

namespace Sapienza.Lexus.advProcessosDadosHerdeiros;

public class advProcessosDadosHerdeirosAutoMapperProfile : Profile
{
    public advProcessosDadosHerdeirosAutoMapperProfile()
    {
        CreateMap<advProcessosDadosHerdeiros, advProcessosDadosHerdeirosDto>();
        CreateMap<CreateUpdateadvProcessosDadosHerdeirosDto, advProcessosDadosHerdeiros>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProcessosDadosHerdeirosDto, advProcessosDadosHerdeiros>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
