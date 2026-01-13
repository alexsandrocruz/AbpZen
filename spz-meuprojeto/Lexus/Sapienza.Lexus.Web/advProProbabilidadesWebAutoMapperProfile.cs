using AutoMapper;
using Sapienza.Lexus.advProProbabilidades.Dtos;
using Sapienza.Lexus.Web.Pages.advProProbabilidades.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProProbabilidadesWebAutoMapperProfile : Profile
{
    public advProProbabilidadesWebAutoMapperProfile()
    {
        CreateMap<advProProbabilidadesDto, EditadvProProbabilidadesViewModel>();
        CreateMap<CreateadvProProbabilidadesViewModel, CreateUpdateadvProProbabilidadesDto>();
        CreateMap<EditadvProProbabilidadesViewModel, CreateUpdateadvProProbabilidadesDto>();
    }
}
