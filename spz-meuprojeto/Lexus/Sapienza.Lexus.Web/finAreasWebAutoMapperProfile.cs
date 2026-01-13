using AutoMapper;
using Sapienza.Lexus.finAreas.Dtos;
using Sapienza.Lexus.Web.Pages.finAreas.ViewModels;

namespace Sapienza.Lexus.Web;

public class finAreasWebAutoMapperProfile : Profile
{
    public finAreasWebAutoMapperProfile()
    {
        CreateMap<finAreasDto, EditfinAreasViewModel>();
        CreateMap<CreatefinAreasViewModel, CreateUpdatefinAreasDto>();
        CreateMap<EditfinAreasViewModel, CreateUpdatefinAreasDto>();
    }
}
