using AutoMapper;
using Sapienza.Lexus.advAgeTiposCompromissos.Dtos;
using Sapienza.Lexus.Web.Pages.advAgeTiposCompromissos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advAgeTiposCompromissosWebAutoMapperProfile : Profile
{
    public advAgeTiposCompromissosWebAutoMapperProfile()
    {
        CreateMap<advAgeTiposCompromissosDto, EditadvAgeTiposCompromissosViewModel>();
        CreateMap<CreateadvAgeTiposCompromissosViewModel, CreateUpdateadvAgeTiposCompromissosDto>();
        CreateMap<EditadvAgeTiposCompromissosViewModel, CreateUpdateadvAgeTiposCompromissosDto>();
    }
}
