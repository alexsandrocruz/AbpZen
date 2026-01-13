using AutoMapper;
using Sapienza.Lexus.advClientesINSS.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesINSS.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesINSSWebAutoMapperProfile : Profile
{
    public advClientesINSSWebAutoMapperProfile()
    {
        CreateMap<advClientesINSSDto, EditadvClientesINSSViewModel>();
        CreateMap<CreateadvClientesINSSViewModel, CreateUpdateadvClientesINSSDto>();
        CreateMap<EditadvClientesINSSViewModel, CreateUpdateadvClientesINSSDto>();
    }
}
