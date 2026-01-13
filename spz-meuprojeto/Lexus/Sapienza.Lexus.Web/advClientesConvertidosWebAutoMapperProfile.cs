using AutoMapper;
using Sapienza.Lexus.advClientesConvertidos.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesConvertidos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesConvertidosWebAutoMapperProfile : Profile
{
    public advClientesConvertidosWebAutoMapperProfile()
    {
        CreateMap<advClientesConvertidosDto, EditadvClientesConvertidosViewModel>();
        CreateMap<CreateadvClientesConvertidosViewModel, CreateUpdateadvClientesConvertidosDto>();
        CreateMap<EditadvClientesConvertidosViewModel, CreateUpdateadvClientesConvertidosDto>();
    }
}
