using AutoMapper;
using Sapienza.Lexus.flwAcoes.Dtos;
using Sapienza.Lexus.Web.Pages.flwAcoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class flwAcoesWebAutoMapperProfile : Profile
{
    public flwAcoesWebAutoMapperProfile()
    {
        CreateMap<flwAcoesDto, EditflwAcoesViewModel>();
        CreateMap<CreateflwAcoesViewModel, CreateUpdateflwAcoesDto>();
        CreateMap<EditflwAcoesViewModel, CreateUpdateflwAcoesDto>();
    }
}
