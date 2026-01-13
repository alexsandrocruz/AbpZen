using AutoMapper;
using Sapienza.Lexus.opoTipos.Dtos;
using Sapienza.Lexus.Web.Pages.opoTipos.ViewModels;

namespace Sapienza.Lexus.Web;

public class opoTiposWebAutoMapperProfile : Profile
{
    public opoTiposWebAutoMapperProfile()
    {
        CreateMap<opoTiposDto, EditopoTiposViewModel>();
        CreateMap<CreateopoTiposViewModel, CreateUpdateopoTiposDto>();
        CreateMap<EditopoTiposViewModel, CreateUpdateopoTiposDto>();
    }
}
