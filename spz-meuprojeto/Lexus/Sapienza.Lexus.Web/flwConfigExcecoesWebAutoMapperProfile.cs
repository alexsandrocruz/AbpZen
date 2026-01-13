using AutoMapper;
using Sapienza.Lexus.flwConfigExcecoes.Dtos;
using Sapienza.Lexus.Web.Pages.flwConfigExcecoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class flwConfigExcecoesWebAutoMapperProfile : Profile
{
    public flwConfigExcecoesWebAutoMapperProfile()
    {
        CreateMap<flwConfigExcecoesDto, EditflwConfigExcecoesViewModel>();
        CreateMap<CreateflwConfigExcecoesViewModel, CreateUpdateflwConfigExcecoesDto>();
        CreateMap<EditflwConfigExcecoesViewModel, CreateUpdateflwConfigExcecoesDto>();
    }
}
