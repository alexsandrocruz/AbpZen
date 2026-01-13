using AutoMapper;
using Sapienza.Lexus.advProEscritorios.Dtos;
using Sapienza.Lexus.Web.Pages.advProEscritorios.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProEscritoriosWebAutoMapperProfile : Profile
{
    public advProEscritoriosWebAutoMapperProfile()
    {
        CreateMap<advProEscritoriosDto, EditadvProEscritoriosViewModel>();
        CreateMap<CreateadvProEscritoriosViewModel, CreateUpdateadvProEscritoriosDto>();
        CreateMap<EditadvProEscritoriosViewModel, CreateUpdateadvProEscritoriosDto>();
    }
}
