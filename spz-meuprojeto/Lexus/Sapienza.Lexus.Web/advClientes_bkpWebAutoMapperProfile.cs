using AutoMapper;
using Sapienza.Lexus.advClientes_bkp.Dtos;
using Sapienza.Lexus.Web.Pages.advClientes_bkp.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientes_bkpWebAutoMapperProfile : Profile
{
    public advClientes_bkpWebAutoMapperProfile()
    {
        CreateMap<advClientes_bkpDto, EditadvClientes_bkpViewModel>();
        CreateMap<CreateadvClientes_bkpViewModel, CreateUpdateadvClientes_bkpDto>();
        CreateMap<EditadvClientes_bkpViewModel, CreateUpdateadvClientes_bkpDto>();
    }
}
