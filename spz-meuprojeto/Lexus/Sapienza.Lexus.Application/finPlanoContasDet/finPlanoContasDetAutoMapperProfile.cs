using AutoMapper;
using Sapienza.Lexus.finPlanoContasDet.Dtos;

namespace Sapienza.Lexus.finPlanoContasDet;

public class finPlanoContasDetAutoMapperProfile : Profile
{
    public finPlanoContasDetAutoMapperProfile()
    {
        CreateMap<finPlanoContasDet, finPlanoContasDetDto>();
        CreateMap<CreateUpdatefinPlanoContasDetDto, finPlanoContasDet>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinPlanoContasDetDto, finPlanoContasDet>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
