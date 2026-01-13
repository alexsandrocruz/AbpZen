using AutoMapper;
using Sapienza.Lexus.finContasClientes.Dtos;
using Sapienza.Lexus.Web.Pages.finContasClientes.ViewModels;

namespace Sapienza.Lexus.Web;

public class finContasClientesWebAutoMapperProfile : Profile
{
    public finContasClientesWebAutoMapperProfile()
    {
        CreateMap<finContasClientesDto, EditfinContasClientesViewModel>();
        CreateMap<CreatefinContasClientesViewModel, CreateUpdatefinContasClientesDto>();
        CreateMap<EditfinContasClientesViewModel, CreateUpdatefinContasClientesDto>();
    }
}
