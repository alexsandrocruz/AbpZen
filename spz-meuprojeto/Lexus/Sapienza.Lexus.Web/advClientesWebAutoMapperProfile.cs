using AutoMapper;
using Sapienza.Lexus.advClientes.Dtos;
using Sapienza.Lexus.Web.Pages.advClientes.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesWebAutoMapperProfile : Profile
{
    public advClientesWebAutoMapperProfile()
    {
        CreateMap<advClientesDto, EditadvClientesViewModel>();
        CreateMap<CreateadvClientesViewModel, CreateUpdateadvClientesDto>();
        CreateMap<EditadvClientesViewModel, CreateUpdateadvClientesDto>();
    }
}
