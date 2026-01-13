using AutoMapper;
using Sapienza.Lexus.advAgeTiposTarefas.Dtos;
using Sapienza.Lexus.Web.Pages.advAgeTiposTarefas.ViewModels;

namespace Sapienza.Lexus.Web;

public class advAgeTiposTarefasWebAutoMapperProfile : Profile
{
    public advAgeTiposTarefasWebAutoMapperProfile()
    {
        CreateMap<advAgeTiposTarefasDto, EditadvAgeTiposTarefasViewModel>();
        CreateMap<CreateadvAgeTiposTarefasViewModel, CreateUpdateadvAgeTiposTarefasDto>();
        CreateMap<EditadvAgeTiposTarefasViewModel, CreateUpdateadvAgeTiposTarefasDto>();
    }
}
