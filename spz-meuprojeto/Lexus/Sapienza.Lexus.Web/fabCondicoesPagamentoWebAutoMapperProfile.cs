using AutoMapper;
using Sapienza.Lexus.fabCondicoesPagamento.Dtos;
using Sapienza.Lexus.Web.Pages.fabCondicoesPagamento.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabCondicoesPagamentoWebAutoMapperProfile : Profile
{
    public fabCondicoesPagamentoWebAutoMapperProfile()
    {
        CreateMap<fabCondicoesPagamentoDto, EditfabCondicoesPagamentoViewModel>();
        CreateMap<CreatefabCondicoesPagamentoViewModel, CreateUpdatefabCondicoesPagamentoDto>();
        CreateMap<EditfabCondicoesPagamentoViewModel, CreateUpdatefabCondicoesPagamentoDto>();
    }
}
