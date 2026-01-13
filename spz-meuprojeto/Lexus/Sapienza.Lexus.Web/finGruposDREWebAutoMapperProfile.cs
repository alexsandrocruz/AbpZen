using AutoMapper;
using Sapienza.Lexus.finGruposDRE.Dtos;
using Sapienza.Lexus.Web.Pages.finGruposDRE.ViewModels;

namespace Sapienza.Lexus.Web;

public class finGruposDREWebAutoMapperProfile : Profile
{
    public finGruposDREWebAutoMapperProfile()
    {
        CreateMap<finGruposDREDto, EditfinGruposDREViewModel>();
        CreateMap<CreatefinGruposDREViewModel, CreateUpdatefinGruposDREDto>();
        CreateMap<EditfinGruposDREViewModel, CreateUpdatefinGruposDREDto>();
    }
}
