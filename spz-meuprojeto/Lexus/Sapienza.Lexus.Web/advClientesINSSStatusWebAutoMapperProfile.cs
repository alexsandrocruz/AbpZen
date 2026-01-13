using AutoMapper;
using Sapienza.Lexus.advClientesINSSStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesINSSStatus.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesINSSStatusWebAutoMapperProfile : Profile
{
    public advClientesINSSStatusWebAutoMapperProfile()
    {
        CreateMap<advClientesINSSStatusDto, EditadvClientesINSSStatusViewModel>();
        CreateMap<CreateadvClientesINSSStatusViewModel, CreateUpdateadvClientesINSSStatusDto>();
        CreateMap<EditadvClientesINSSStatusViewModel, CreateUpdateadvClientesINSSStatusDto>();
    }
}
