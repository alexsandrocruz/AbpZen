using AutoMapper;
using Sapienza.Lexus.advTarefas.Dtos;
using Sapienza.Lexus.Web.Pages.advTarefas.ViewModels;

namespace Sapienza.Lexus.Web;

public class advTarefasWebAutoMapperProfile : Profile
{
    public advTarefasWebAutoMapperProfile()
    {
        CreateMap<advTarefasDto, EditadvTarefasViewModel>();
        CreateMap<CreateadvTarefasViewModel, CreateUpdateadvTarefasDto>();
        CreateMap<EditadvTarefasViewModel, CreateUpdateadvTarefasDto>();
    }
}
