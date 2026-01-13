using AutoMapper;
using Sapienza.Lexus.usuAcessos.Dtos;
using Sapienza.Lexus.Web.Pages.usuAcessos.ViewModels;

namespace Sapienza.Lexus.Web;

public class usuAcessosWebAutoMapperProfile : Profile
{
    public usuAcessosWebAutoMapperProfile()
    {
        CreateMap<usuAcessosDto, EditusuAcessosViewModel>();
        CreateMap<CreateusuAcessosViewModel, CreateUpdateusuAcessosDto>();
        CreateMap<EditusuAcessosViewModel, CreateUpdateusuAcessosDto>();
    }
}
