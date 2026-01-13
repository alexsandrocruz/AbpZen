using AutoMapper;
using Sapienza.Lexus.usuAreas.Dtos;
using Sapienza.Lexus.Web.Pages.usuAreas.ViewModels;

namespace Sapienza.Lexus.Web;

public class usuAreasWebAutoMapperProfile : Profile
{
    public usuAreasWebAutoMapperProfile()
    {
        CreateMap<usuAreasDto, EditusuAreasViewModel>();
        CreateMap<CreateusuAreasViewModel, CreateUpdateusuAreasDto>();
        CreateMap<EditusuAreasViewModel, CreateUpdateusuAreasDto>();
    }
}
