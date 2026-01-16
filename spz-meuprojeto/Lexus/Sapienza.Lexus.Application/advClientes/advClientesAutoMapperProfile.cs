using AutoMapper;
using Sapienza.Lexus.advClientes.Dtos;

namespace Sapienza.Lexus.advClientes;

public class advClientesAutoMapperProfile : Profile
{
    public advClientesAutoMapperProfile()
    {
        CreateMap<advClientes, advClientesDto>()
            .ForMember(dest => dest.advClientesArquivosDisplayName, opt => opt.MapFrom(src => src.advClientesNav.descricao))
            .ForMember(dest => dest.advClientesAtualizacoesDisplayName, opt => opt.MapFrom(src => src.advClientesNav1.campo))
            .ForMember(dest => dest.advClientesChecklistDisplayName, opt => opt.MapFrom(src => src.advClientesNav2.Id))
            .ForMember(dest => dest.advProcessosDisplayName, opt => opt.MapFrom(src => src.advClientesNav3.sintese))
            .ForMember(dest => dest.advProcessosClientesDisplayName, opt => opt.MapFrom(src => src.advClientesNav4.Id))
            .ForMember(dest => dest.advClientesHistoricosDisplayName, opt => opt.MapFrom(src => src.advClientesNav5.data))
            .ForMember(dest => dest.opoOportunidadesDisplayName, opt => opt.MapFrom(src => src.advClientesNav6.titulo))
            .ForMember(dest => dest.flwFollowsDisplayName, opt => opt.MapFrom(src => src.advClientesNav7.data));
        CreateMap<CreateUpdateadvClientesDto, advClientes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesDto, advClientes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
