using AutoMapper;
using Sapienza.Lexus.advPostosINSS.Dtos;
using Sapienza.Lexus.Web.Pages.advPostosINSS.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPostosINSSWebAutoMapperProfile : Profile
{
    public advPostosINSSWebAutoMapperProfile()
    {
        CreateMap<advPostosINSSDto, EditadvPostosINSSViewModel>();
        CreateMap<CreateadvPostosINSSViewModel, CreateUpdateadvPostosINSSDto>();
        CreateMap<EditadvPostosINSSViewModel, CreateUpdateadvPostosINSSDto>();
    }
}
