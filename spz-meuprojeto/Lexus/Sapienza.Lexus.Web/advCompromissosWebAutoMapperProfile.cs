using AutoMapper;
using Sapienza.Lexus.advCompromissos.Dtos;
using Sapienza.Lexus.Web.Pages.advCompromissos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCompromissosWebAutoMapperProfile : Profile
{
    public advCompromissosWebAutoMapperProfile()
    {
        CreateMap<advCompromissosDto, EditadvCompromissosViewModel>();
        CreateMap<CreateadvCompromissosViewModel, CreateUpdateadvCompromissosDto>();
        CreateMap<EditadvCompromissosViewModel, CreateUpdateadvCompromissosDto>();
    }
}
