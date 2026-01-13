using AutoMapper;
using Sapienza.Lexus.fabFormasPagamento.Dtos;
using Sapienza.Lexus.Web.Pages.fabFormasPagamento.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabFormasPagamentoWebAutoMapperProfile : Profile
{
    public fabFormasPagamentoWebAutoMapperProfile()
    {
        CreateMap<fabFormasPagamentoDto, EditfabFormasPagamentoViewModel>();
        CreateMap<CreatefabFormasPagamentoViewModel, CreateUpdatefabFormasPagamentoDto>();
        CreateMap<EditfabFormasPagamentoViewModel, CreateUpdatefabFormasPagamentoDto>();
    }
}
