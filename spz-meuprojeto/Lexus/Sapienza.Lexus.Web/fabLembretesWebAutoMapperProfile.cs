using AutoMapper;
using Sapienza.Lexus.fabLembretes.Dtos;
using Sapienza.Lexus.Web.Pages.fabLembretes.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabLembretesWebAutoMapperProfile : Profile
{
    public fabLembretesWebAutoMapperProfile()
    {
        CreateMap<fabLembretesDto, EditfabLembretesViewModel>();
        CreateMap<CreatefabLembretesViewModel, CreateUpdatefabLembretesDto>();
        CreateMap<EditfabLembretesViewModel, CreateUpdatefabLembretesDto>();
    }
}
