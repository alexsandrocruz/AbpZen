using AutoMapper;
using Sapienza.Lexus.advTarefasAtualizacoes.Dtos;
using Sapienza.Lexus.Web.Pages.advTarefasAtualizacoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class advTarefasAtualizacoesWebAutoMapperProfile : Profile
{
    public advTarefasAtualizacoesWebAutoMapperProfile()
    {
        CreateMap<advTarefasAtualizacoesDto, EditadvTarefasAtualizacoesViewModel>();
        CreateMap<CreateadvTarefasAtualizacoesViewModel, CreateUpdateadvTarefasAtualizacoesDto>();
        CreateMap<EditadvTarefasAtualizacoesViewModel, CreateUpdateadvTarefasAtualizacoesDto>();
    }
}
