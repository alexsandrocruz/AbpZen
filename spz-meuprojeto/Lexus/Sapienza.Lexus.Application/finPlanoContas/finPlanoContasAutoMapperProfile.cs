using AutoMapper;
using Sapienza.Lexus.finPlanoContas.Dtos;

namespace Sapienza.Lexus.finPlanoContas;

public class finPlanoContasAutoMapperProfile : Profile
{
    public finPlanoContasAutoMapperProfile()
    {
        CreateMap<finPlanoContas, finPlanoContasDto>();
        CreateMap<CreateUpdatefinPlanoContasDto, finPlanoContas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinPlanoContasDto, finPlanoContas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
