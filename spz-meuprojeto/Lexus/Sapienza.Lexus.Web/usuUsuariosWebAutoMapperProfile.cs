using AutoMapper;
using Sapienza.Lexus.usuUsuarios.Dtos;
using Sapienza.Lexus.Web.Pages.usuUsuarios.ViewModels;

namespace Sapienza.Lexus.Web;

public class usuUsuariosWebAutoMapperProfile : Profile
{
    public usuUsuariosWebAutoMapperProfile()
    {
        CreateMap<usuUsuariosDto, EditusuUsuariosViewModel>();
        CreateMap<CreateusuUsuariosViewModel, CreateUpdateusuUsuariosDto>();
        CreateMap<EditusuUsuariosViewModel, CreateUpdateusuUsuariosDto>();
    }
}
