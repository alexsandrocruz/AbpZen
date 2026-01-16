using AutoMapper;
using Sapienza.Lexus.advProcessos.Dtos;

namespace Sapienza.Lexus.advProcessos;

public class advProcessosAutoMapperProfile : Profile
{
    public advProcessosAutoMapperProfile()
    {
        CreateMap<advProcessos, advProcessosDto>()
            .ForMember(dest => dest.advProcessosClientesDisplayName, opt => opt.MapFrom(src => src.advProcessosNav.Id))
            .ForMember(dest => dest.advProcessosHonorariosDisplayName, opt => opt.MapFrom(src => src.advProcessosNav1.dataPrevistaClienteReceber))
            .ForMember(dest => dest.advProcessosMeritosDisplayName, opt => opt.MapFrom(src => src.advProcessosNav2.Id))
            .ForMember(dest => dest.advCompromissosDisplayName, opt => opt.MapFrom(src => src.advProcessosNav3.dataPublicacao))
            .ForMember(dest => dest.advProcessosAlteracoesDisplayName, opt => opt.MapFrom(src => src.advProcessosNav4.texto))
            .ForMember(dest => dest.advProcessosDadosHerdeirosDisplayName, opt => opt.MapFrom(src => src.advProcessosNav5.bancarioTipoConta))
            .ForMember(dest => dest.advTarefasDisplayName, opt => opt.MapFrom(src => src.advProcessosNav6.dataCadastro))
            .ForMember(dest => dest.advVerbasDisplayName, opt => opt.MapFrom(src => src.advProcessosNav7.dataDe));
        CreateMap<CreateUpdateadvProcessosDto, advProcessos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProcessosDto, advProcessos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
