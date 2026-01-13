using AutoMapper;
using Sapienza.Lexus.advCliComoChegou.Dtos;
using Sapienza.Lexus.Web.Pages.advCliComoChegou.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliComoChegouWebAutoMapperProfile : Profile
{
    public advCliComoChegouWebAutoMapperProfile()
    {
        CreateMap<advCliComoChegouDto, EditadvCliComoChegouViewModel>();
        CreateMap<CreateadvCliComoChegouViewModel, CreateUpdateadvCliComoChegouDto>();
        CreateMap<EditadvCliComoChegouViewModel, CreateUpdateadvCliComoChegouDto>();
    }
}
