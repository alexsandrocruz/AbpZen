using AutoMapper;
using Sapienza.Lexus.finRateiosPadrao.Dtos;
using Sapienza.Lexus.Web.Pages.finRateiosPadrao.ViewModels;

namespace Sapienza.Lexus.Web;

public class finRateiosPadraoWebAutoMapperProfile : Profile
{
    public finRateiosPadraoWebAutoMapperProfile()
    {
        CreateMap<finRateiosPadraoDto, EditfinRateiosPadraoViewModel>();
        CreateMap<CreatefinRateiosPadraoViewModel, CreateUpdatefinRateiosPadraoDto>();
        CreateMap<EditfinRateiosPadraoViewModel, CreateUpdatefinRateiosPadraoDto>();
    }
}
