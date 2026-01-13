using AutoMapper;
using Sapienza.Lexus.fabPaises.Dtos;
using Sapienza.Lexus.Web.Pages.fabPaises.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabPaisesWebAutoMapperProfile : Profile
{
    public fabPaisesWebAutoMapperProfile()
    {
        CreateMap<fabPaisesDto, EditfabPaisesViewModel>();
        CreateMap<CreatefabPaisesViewModel, CreateUpdatefabPaisesDto>();
        CreateMap<EditfabPaisesViewModel, CreateUpdatefabPaisesDto>();
    }
}
