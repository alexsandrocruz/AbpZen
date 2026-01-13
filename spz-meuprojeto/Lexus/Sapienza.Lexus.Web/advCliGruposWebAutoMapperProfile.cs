using AutoMapper;
using Sapienza.Lexus.advCliGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliGrupos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliGruposWebAutoMapperProfile : Profile
{
    public advCliGruposWebAutoMapperProfile()
    {
        CreateMap<advCliGruposDto, EditadvCliGruposViewModel>();
        CreateMap<CreateadvCliGruposViewModel, CreateUpdateadvCliGruposDto>();
        CreateMap<EditadvCliGruposViewModel, CreateUpdateadvCliGruposDto>();
    }
}
