using AutoMapper;
using Sapienza.Lexus.fabPermissoes.Dtos;
using Sapienza.Lexus.Web.Pages.fabPermissoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabPermissoesWebAutoMapperProfile : Profile
{
    public fabPermissoesWebAutoMapperProfile()
    {
        CreateMap<fabPermissoesDto, EditfabPermissoesViewModel>();
        CreateMap<CreatefabPermissoesViewModel, CreateUpdatefabPermissoesDto>();
        CreateMap<EditfabPermissoesViewModel, CreateUpdatefabPermissoesDto>();
    }
}
