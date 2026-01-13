using AutoMapper;
using Sapienza.Lexus.advPautaObs.Dtos;
using Sapienza.Lexus.Web.Pages.advPautaObs.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPautaObsWebAutoMapperProfile : Profile
{
    public advPautaObsWebAutoMapperProfile()
    {
        CreateMap<advPautaObsDto, EditadvPautaObsViewModel>();
        CreateMap<CreateadvPautaObsViewModel, CreateUpdateadvPautaObsDto>();
        CreateMap<EditadvPautaObsViewModel, CreateUpdateadvPautaObsDto>();
    }
}
