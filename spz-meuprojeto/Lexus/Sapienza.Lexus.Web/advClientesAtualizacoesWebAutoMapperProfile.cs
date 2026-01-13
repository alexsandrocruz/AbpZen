using AutoMapper;
using Sapienza.Lexus.advClientesAtualizacoes.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesAtualizacoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesAtualizacoesWebAutoMapperProfile : Profile
{
    public advClientesAtualizacoesWebAutoMapperProfile()
    {
        CreateMap<advClientesAtualizacoesDto, EditadvClientesAtualizacoesViewModel>();
        CreateMap<CreateadvClientesAtualizacoesViewModel, CreateUpdateadvClientesAtualizacoesDto>();
        CreateMap<EditadvClientesAtualizacoesViewModel, CreateUpdateadvClientesAtualizacoesDto>();
    }
}
