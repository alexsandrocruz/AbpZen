using AutoMapper;
using Sapienza.Lexus.finRateios.Dtos;
using Sapienza.Lexus.Web.Pages.finRateios.ViewModels;

namespace Sapienza.Lexus.Web;

public class finRateiosWebAutoMapperProfile : Profile
{
    public finRateiosWebAutoMapperProfile()
    {
        CreateMap<finRateiosDto, EditfinRateiosViewModel>();
        CreateMap<CreatefinRateiosViewModel, CreateUpdatefinRateiosDto>();
        CreateMap<EditfinRateiosViewModel, CreateUpdatefinRateiosDto>();
    }
}
