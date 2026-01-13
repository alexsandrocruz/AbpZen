using AutoMapper;
using Sapienza.Lexus.fabConfig.Dtos;
using Sapienza.Lexus.Web.Pages.fabConfig.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabConfigWebAutoMapperProfile : Profile
{
    public fabConfigWebAutoMapperProfile()
    {
        CreateMap<fabConfigDto, EditfabConfigViewModel>();
        CreateMap<CreatefabConfigViewModel, CreateUpdatefabConfigDto>();
        CreateMap<EditfabConfigViewModel, CreateUpdatefabConfigDto>();
    }
}
