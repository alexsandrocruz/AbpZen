using AutoMapper;
using Sapienza.Lexus.advPreCheckListsGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.advPreCheckListsGrupos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreCheckListsGruposWebAutoMapperProfile : Profile
{
    public advPreCheckListsGruposWebAutoMapperProfile()
    {
        CreateMap<advPreCheckListsGruposDto, EditadvPreCheckListsGruposViewModel>();
        CreateMap<CreateadvPreCheckListsGruposViewModel, CreateUpdateadvPreCheckListsGruposDto>();
        CreateMap<EditadvPreCheckListsGruposViewModel, CreateUpdateadvPreCheckListsGruposDto>();
    }
}
