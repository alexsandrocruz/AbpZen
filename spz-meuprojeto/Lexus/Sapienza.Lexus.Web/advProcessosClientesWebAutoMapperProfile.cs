using AutoMapper;
using Sapienza.Lexus.advProcessosClientes.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosClientes.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProcessosClientesWebAutoMapperProfile : Profile
{
    public advProcessosClientesWebAutoMapperProfile()
    {
        CreateMap<advProcessosClientesDto, EditadvProcessosClientesViewModel>();
        CreateMap<CreateadvProcessosClientesViewModel, CreateUpdateadvProcessosClientesDto>();
        CreateMap<EditadvProcessosClientesViewModel, CreateUpdateadvProcessosClientesDto>();
    }
}
