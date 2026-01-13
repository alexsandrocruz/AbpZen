using AutoMapper;
using Sapienza.Lexus.flwFollows.Dtos;
using Sapienza.Lexus.Web.Pages.flwFollows.ViewModels;

namespace Sapienza.Lexus.Web;

public class flwFollowsWebAutoMapperProfile : Profile
{
    public flwFollowsWebAutoMapperProfile()
    {
        CreateMap<flwFollowsDto, EditflwFollowsViewModel>();
        CreateMap<CreateflwFollowsViewModel, CreateUpdateflwFollowsDto>();
        CreateMap<EditflwFollowsViewModel, CreateUpdateflwFollowsDto>();
    }
}
