using AutoMapper;
using Sapienza.Lexus.advCliCargos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliCargos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliCargosWebAutoMapperProfile : Profile
{
    public advCliCargosWebAutoMapperProfile()
    {
        CreateMap<advCliCargosDto, EditadvCliCargosViewModel>();
        CreateMap<CreateadvCliCargosViewModel, CreateUpdateadvCliCargosDto>();
        CreateMap<EditadvCliCargosViewModel, CreateUpdateadvCliCargosDto>();
    }
}
