using AutoMapper;
using Sapienza.Lexus.finPrestacaoContas.Dtos;

namespace Sapienza.Lexus.finPrestacaoContas;

public class finPrestacaoContasAutoMapperProfile : Profile
{
    public finPrestacaoContasAutoMapperProfile()
    {
        CreateMap<finPrestacaoContas, finPrestacaoContasDto>();
        CreateMap<CreateUpdatefinPrestacaoContasDto, finPrestacaoContas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinPrestacaoContasDto, finPrestacaoContas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
