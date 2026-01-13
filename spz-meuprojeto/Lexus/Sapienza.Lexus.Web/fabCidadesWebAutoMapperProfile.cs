using AutoMapper;
using Sapienza.Lexus.fabCidades.Dtos;
using Sapienza.Lexus.Web.Pages.fabCidades.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabCidadesWebAutoMapperProfile : Profile
{
    public fabCidadesWebAutoMapperProfile()
    {
        CreateMap<fabCidadesDto, EditfabCidadesViewModel>();
        CreateMap<CreatefabCidadesViewModel, CreateUpdatefabCidadesDto>();
        CreateMap<EditfabCidadesViewModel, CreateUpdatefabCidadesDto>();
    }
}
