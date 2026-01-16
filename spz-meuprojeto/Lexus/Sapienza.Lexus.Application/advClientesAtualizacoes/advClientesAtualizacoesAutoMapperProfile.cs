using AutoMapper;
using Sapienza.Lexus.advClientesAtualizacoes.Dtos;

namespace Sapienza.Lexus.advClientesAtualizacoes;

public class advClientesAtualizacoesAutoMapperProfile : Profile
{
    public advClientesAtualizacoesAutoMapperProfile()
    {
        CreateMap<advClientesAtualizacoes, advClientesAtualizacoesDto>();
        CreateMap<CreateUpdateadvClientesAtualizacoesDto, advClientesAtualizacoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesAtualizacoesDto, advClientesAtualizacoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
