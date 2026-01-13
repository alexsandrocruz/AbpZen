using AutoMapper;
using Sapienza.Lexus.advCliLocaisAtendido.Dtos;
using Sapienza.Lexus.Web.Pages.advCliLocaisAtendido.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliLocaisAtendidoWebAutoMapperProfile : Profile
{
    public advCliLocaisAtendidoWebAutoMapperProfile()
    {
        CreateMap<advCliLocaisAtendidoDto, EditadvCliLocaisAtendidoViewModel>();
        CreateMap<CreateadvCliLocaisAtendidoViewModel, CreateUpdateadvCliLocaisAtendidoDto>();
        CreateMap<EditadvCliLocaisAtendidoViewModel, CreateUpdateadvCliLocaisAtendidoDto>();
    }
}
