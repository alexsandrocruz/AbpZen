using AutoMapper;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;
using Sapienza.Lexus.Web.Pages.fabHistoricoTipos.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabHistoricoTiposWebAutoMapperProfile : Profile
{
    public fabHistoricoTiposWebAutoMapperProfile()
    {
        CreateMap<fabHistoricoTiposDto, EditfabHistoricoTiposViewModel>();
        CreateMap<CreatefabHistoricoTiposViewModel, CreateUpdatefabHistoricoTiposDto>();
        CreateMap<EditfabHistoricoTiposViewModel, CreateUpdatefabHistoricoTiposDto>();
    }
}
