using AutoMapper;
using Sapienza.Lexus.advClientesModelos.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesModelos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesModelosWebAutoMapperProfile : Profile
{
    public advClientesModelosWebAutoMapperProfile()
    {
        CreateMap<advClientesModelosDto, EditadvClientesModelosViewModel>();
        CreateMap<CreateadvClientesModelosViewModel, CreateUpdateadvClientesModelosDto>();
        CreateMap<EditadvClientesModelosViewModel, CreateUpdateadvClientesModelosDto>();
    }
}
