using AutoMapper;
using Sapienza.Lexus.advCliBairros.Dtos;
using Sapienza.Lexus.Web.Pages.advCliBairros.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliBairrosWebAutoMapperProfile : Profile
{
    public advCliBairrosWebAutoMapperProfile()
    {
        CreateMap<advCliBairrosDto, EditadvCliBairrosViewModel>();
        CreateMap<CreateadvCliBairrosViewModel, CreateUpdateadvCliBairrosDto>();
        CreateMap<EditadvCliBairrosViewModel, CreateUpdateadvCliBairrosDto>();
    }
}
