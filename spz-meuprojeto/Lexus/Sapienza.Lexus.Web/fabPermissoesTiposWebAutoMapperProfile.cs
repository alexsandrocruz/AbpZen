using AutoMapper;
using Sapienza.Lexus.fabPermissoesTipos.Dtos;
using Sapienza.Lexus.Web.Pages.fabPermissoesTipos.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabPermissoesTiposWebAutoMapperProfile : Profile
{
    public fabPermissoesTiposWebAutoMapperProfile()
    {
        CreateMap<fabPermissoesTiposDto, EditfabPermissoesTiposViewModel>();
        CreateMap<CreatefabPermissoesTiposViewModel, CreateUpdatefabPermissoesTiposDto>();
        CreateMap<EditfabPermissoesTiposViewModel, CreateUpdatefabPermissoesTiposDto>();
    }
}
