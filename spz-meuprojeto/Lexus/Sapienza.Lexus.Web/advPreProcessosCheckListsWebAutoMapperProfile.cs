using AutoMapper;
using Sapienza.Lexus.advPreProcessosCheckLists.Dtos;
using Sapienza.Lexus.Web.Pages.advPreProcessosCheckLists.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreProcessosCheckListsWebAutoMapperProfile : Profile
{
    public advPreProcessosCheckListsWebAutoMapperProfile()
    {
        CreateMap<advPreProcessosCheckListsDto, EditadvPreProcessosCheckListsViewModel>();
        CreateMap<CreateadvPreProcessosCheckListsViewModel, CreateUpdateadvPreProcessosCheckListsDto>();
        CreateMap<EditadvPreProcessosCheckListsViewModel, CreateUpdateadvPreProcessosCheckListsDto>();
    }
}
