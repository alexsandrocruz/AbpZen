using AutoMapper;
using Sapienza.Lexus.advCliTiposHistoricos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliTiposHistoricos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliTiposHistoricosWebAutoMapperProfile : Profile
{
    public advCliTiposHistoricosWebAutoMapperProfile()
    {
        CreateMap<advCliTiposHistoricosDto, EditadvCliTiposHistoricosViewModel>();
        CreateMap<CreateadvCliTiposHistoricosViewModel, CreateUpdateadvCliTiposHistoricosDto>();
        CreateMap<EditadvCliTiposHistoricosViewModel, CreateUpdateadvCliTiposHistoricosDto>();
    }
}
