using AutoMapper;
using Sapienza.Lexus.fabFormasRecebimento.Dtos;
using Sapienza.Lexus.Web.Pages.fabFormasRecebimento.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabFormasRecebimentoWebAutoMapperProfile : Profile
{
    public fabFormasRecebimentoWebAutoMapperProfile()
    {
        CreateMap<fabFormasRecebimentoDto, EditfabFormasRecebimentoViewModel>();
        CreateMap<CreatefabFormasRecebimentoViewModel, CreateUpdatefabFormasRecebimentoDto>();
        CreateMap<EditfabFormasRecebimentoViewModel, CreateUpdatefabFormasRecebimentoDto>();
    }
}
