using AutoMapper;
using Sapienza.Lexus.finRecibos.Dtos;
using Sapienza.Lexus.Web.Pages.finRecibos.ViewModels;

namespace Sapienza.Lexus.Web;

public class finRecibosWebAutoMapperProfile : Profile
{
    public finRecibosWebAutoMapperProfile()
    {
        CreateMap<finRecibosDto, EditfinRecibosViewModel>();
        CreateMap<CreatefinRecibosViewModel, CreateUpdatefinRecibosDto>();
        CreateMap<EditfinRecibosViewModel, CreateUpdatefinRecibosDto>();
    }
}
