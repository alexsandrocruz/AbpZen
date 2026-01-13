using AutoMapper;
using Sapienza.Lexus.advCliPrioridades.Dtos;
using Sapienza.Lexus.Web.Pages.advCliPrioridades.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliPrioridadesWebAutoMapperProfile : Profile
{
    public advCliPrioridadesWebAutoMapperProfile()
    {
        CreateMap<advCliPrioridadesDto, EditadvCliPrioridadesViewModel>();
        CreateMap<CreateadvCliPrioridadesViewModel, CreateUpdateadvCliPrioridadesDto>();
        CreateMap<EditadvCliPrioridadesViewModel, CreateUpdateadvCliPrioridadesDto>();
    }
}
