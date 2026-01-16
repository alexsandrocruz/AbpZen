using AutoMapper;
using Sapienza.Lexus.advTarefasAtualizacoes.Dtos;

namespace Sapienza.Lexus.advTarefasAtualizacoes;

public class advTarefasAtualizacoesAutoMapperProfile : Profile
{
    public advTarefasAtualizacoesAutoMapperProfile()
    {
        CreateMap<advTarefasAtualizacoes, advTarefasAtualizacoesDto>();
        CreateMap<CreateUpdateadvTarefasAtualizacoesDto, advTarefasAtualizacoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvTarefasAtualizacoesDto, advTarefasAtualizacoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
