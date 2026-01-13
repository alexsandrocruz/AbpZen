using AutoMapper;
using Sapienza.Lexus.advClientesChecklist.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesChecklist.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesChecklistWebAutoMapperProfile : Profile
{
    public advClientesChecklistWebAutoMapperProfile()
    {
        CreateMap<advClientesChecklistDto, EditadvClientesChecklistViewModel>();
        CreateMap<CreateadvClientesChecklistViewModel, CreateUpdateadvClientesChecklistDto>();
        CreateMap<EditadvClientesChecklistViewModel, CreateUpdateadvClientesChecklistDto>();
    }
}
