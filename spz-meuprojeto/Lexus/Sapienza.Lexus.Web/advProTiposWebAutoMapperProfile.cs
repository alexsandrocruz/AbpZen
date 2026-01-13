using AutoMapper;
using Sapienza.Lexus.advProTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advProTipos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProTiposWebAutoMapperProfile : Profile
{
    public advProTiposWebAutoMapperProfile()
    {
        CreateMap<advProTiposDto, EditadvProTiposViewModel>();
        CreateMap<CreateadvProTiposViewModel, CreateUpdateadvProTiposDto>();
        CreateMap<EditadvProTiposViewModel, CreateUpdateadvProTiposDto>();
    }
}
