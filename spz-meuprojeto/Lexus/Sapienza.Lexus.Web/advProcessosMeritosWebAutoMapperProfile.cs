using AutoMapper;
using Sapienza.Lexus.advProcessosMeritos.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosMeritos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProcessosMeritosWebAutoMapperProfile : Profile
{
    public advProcessosMeritosWebAutoMapperProfile()
    {
        CreateMap<advProcessosMeritosDto, EditadvProcessosMeritosViewModel>();
        CreateMap<CreateadvProcessosMeritosViewModel, CreateUpdateadvProcessosMeritosDto>();
        CreateMap<EditadvProcessosMeritosViewModel, CreateUpdateadvProcessosMeritosDto>();
    }
}
