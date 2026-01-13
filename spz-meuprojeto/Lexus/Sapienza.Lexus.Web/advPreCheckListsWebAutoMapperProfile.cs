using AutoMapper;
using Sapienza.Lexus.advPreCheckLists.Dtos;
using Sapienza.Lexus.Web.Pages.advPreCheckLists.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreCheckListsWebAutoMapperProfile : Profile
{
    public advPreCheckListsWebAutoMapperProfile()
    {
        CreateMap<advPreCheckListsDto, EditadvPreCheckListsViewModel>();
        CreateMap<CreateadvPreCheckListsViewModel, CreateUpdateadvPreCheckListsDto>();
        CreateMap<EditadvPreCheckListsViewModel, CreateUpdateadvPreCheckListsDto>();
    }
}
