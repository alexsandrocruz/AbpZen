using AutoMapper;
using Sapienza.Lexus.fabRegioes.Dtos;
using Sapienza.Lexus.Web.Pages.fabRegioes.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabRegioesWebAutoMapperProfile : Profile
{
    public fabRegioesWebAutoMapperProfile()
    {
        CreateMap<fabRegioesDto, EditfabRegioesViewModel>();
        CreateMap<CreatefabRegioesViewModel, CreateUpdatefabRegioesDto>();
        CreateMap<EditfabRegioesViewModel, CreateUpdatefabRegioesDto>();
    }
}
