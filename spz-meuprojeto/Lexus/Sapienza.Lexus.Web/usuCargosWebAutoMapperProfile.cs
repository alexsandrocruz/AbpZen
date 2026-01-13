using AutoMapper;
using Sapienza.Lexus.usuCargos.Dtos;
using Sapienza.Lexus.Web.Pages.usuCargos.ViewModels;

namespace Sapienza.Lexus.Web;

public class usuCargosWebAutoMapperProfile : Profile
{
    public usuCargosWebAutoMapperProfile()
    {
        CreateMap<usuCargosDto, EditusuCargosViewModel>();
        CreateMap<CreateusuCargosViewModel, CreateUpdateusuCargosDto>();
        CreateMap<EditusuCargosViewModel, CreateUpdateusuCargosDto>();
    }
}
