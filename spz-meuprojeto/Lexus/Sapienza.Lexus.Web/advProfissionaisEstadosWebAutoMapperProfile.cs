using AutoMapper;
using Sapienza.Lexus.advProfissionaisEstados.Dtos;
using Sapienza.Lexus.Web.Pages.advProfissionaisEstados.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProfissionaisEstadosWebAutoMapperProfile : Profile
{
    public advProfissionaisEstadosWebAutoMapperProfile()
    {
        CreateMap<advProfissionaisEstadosDto, EditadvProfissionaisEstadosViewModel>();
        CreateMap<CreateadvProfissionaisEstadosViewModel, CreateUpdateadvProfissionaisEstadosDto>();
        CreateMap<EditadvProfissionaisEstadosViewModel, CreateUpdateadvProfissionaisEstadosDto>();
    }
}
