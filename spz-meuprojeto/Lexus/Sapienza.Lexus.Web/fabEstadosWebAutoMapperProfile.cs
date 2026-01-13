using AutoMapper;
using Sapienza.Lexus.fabEstados.Dtos;
using Sapienza.Lexus.Web.Pages.fabEstados.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabEstadosWebAutoMapperProfile : Profile
{
    public fabEstadosWebAutoMapperProfile()
    {
        CreateMap<fabEstadosDto, EditfabEstadosViewModel>();
        CreateMap<CreatefabEstadosViewModel, CreateUpdatefabEstadosDto>();
        CreateMap<EditfabEstadosViewModel, CreateUpdatefabEstadosDto>();
    }
}
