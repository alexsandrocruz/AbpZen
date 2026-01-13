using AutoMapper;
using Sapienza.Lexus.advVerTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advVerTipos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advVerTiposWebAutoMapperProfile : Profile
{
    public advVerTiposWebAutoMapperProfile()
    {
        CreateMap<advVerTiposDto, EditadvVerTiposViewModel>();
        CreateMap<CreateadvVerTiposViewModel, CreateUpdateadvVerTiposDto>();
        CreateMap<EditadvVerTiposViewModel, CreateUpdateadvVerTiposDto>();
    }
}
