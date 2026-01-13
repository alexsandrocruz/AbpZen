using AutoMapper;
using Sapienza.Lexus.advProMeritos.Dtos;
using Sapienza.Lexus.Web.Pages.advProMeritos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProMeritosWebAutoMapperProfile : Profile
{
    public advProMeritosWebAutoMapperProfile()
    {
        CreateMap<advProMeritosDto, EditadvProMeritosViewModel>();
        CreateMap<CreateadvProMeritosViewModel, CreateUpdateadvProMeritosDto>();
        CreateMap<EditadvProMeritosViewModel, CreateUpdateadvProMeritosDto>();
    }
}
