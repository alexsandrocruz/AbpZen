using AutoMapper;
using Sapienza.Lexus.advClientesHistoricos.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesHistoricos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesHistoricosWebAutoMapperProfile : Profile
{
    public advClientesHistoricosWebAutoMapperProfile()
    {
        CreateMap<advClientesHistoricosDto, EditadvClientesHistoricosViewModel>();
        CreateMap<CreateadvClientesHistoricosViewModel, CreateUpdateadvClientesHistoricosDto>();
        CreateMap<EditadvClientesHistoricosViewModel, CreateUpdateadvClientesHistoricosDto>();
    }
}
