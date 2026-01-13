using AutoMapper;
using Sapienza.Lexus.fabDatasEFeriados.Dtos;
using Sapienza.Lexus.Web.Pages.fabDatasEFeriados.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabDatasEFeriadosWebAutoMapperProfile : Profile
{
    public fabDatasEFeriadosWebAutoMapperProfile()
    {
        CreateMap<fabDatasEFeriadosDto, EditfabDatasEFeriadosViewModel>();
        CreateMap<CreatefabDatasEFeriadosViewModel, CreateUpdatefabDatasEFeriadosDto>();
        CreateMap<EditfabDatasEFeriadosViewModel, CreateUpdatefabDatasEFeriadosDto>();
    }
}
