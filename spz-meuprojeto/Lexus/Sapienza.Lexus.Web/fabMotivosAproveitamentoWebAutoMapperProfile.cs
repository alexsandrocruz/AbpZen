using AutoMapper;
using Sapienza.Lexus.fabMotivosAproveitamento.Dtos;
using Sapienza.Lexus.Web.Pages.fabMotivosAproveitamento.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabMotivosAproveitamentoWebAutoMapperProfile : Profile
{
    public fabMotivosAproveitamentoWebAutoMapperProfile()
    {
        CreateMap<fabMotivosAproveitamentoDto, EditfabMotivosAproveitamentoViewModel>();
        CreateMap<CreatefabMotivosAproveitamentoViewModel, CreateUpdatefabMotivosAproveitamentoDto>();
        CreateMap<EditfabMotivosAproveitamentoViewModel, CreateUpdatefabMotivosAproveitamentoDto>();
    }
}
