using AutoMapper;
using Sapienza.Lexus.flwConfig.Dtos;
using Sapienza.Lexus.Web.Pages.flwConfig.ViewModels;

namespace Sapienza.Lexus.Web;

public class flwConfigWebAutoMapperProfile : Profile
{
    public flwConfigWebAutoMapperProfile()
    {
        CreateMap<flwConfigDto, EditflwConfigViewModel>();
        CreateMap<CreateflwConfigViewModel, CreateUpdateflwConfigDto>();
        CreateMap<EditflwConfigViewModel, CreateUpdateflwConfigDto>();
    }
}
