using AutoMapper;
using Sapienza.Lexus.finLancamentos_BKP.Dtos;
using Sapienza.Lexus.Web.Pages.finLancamentos_BKP.ViewModels;

namespace Sapienza.Lexus.Web;

public class finLancamentos_BKPWebAutoMapperProfile : Profile
{
    public finLancamentos_BKPWebAutoMapperProfile()
    {
        CreateMap<finLancamentos_BKPDto, EditfinLancamentos_BKPViewModel>();
        CreateMap<CreatefinLancamentos_BKPViewModel, CreateUpdatefinLancamentos_BKPDto>();
        CreateMap<EditfinLancamentos_BKPViewModel, CreateUpdatefinLancamentos_BKPDto>();
    }
}
