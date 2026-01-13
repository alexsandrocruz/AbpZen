using AutoMapper;
using Sapienza.Lexus.advPreStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advPreStatus.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreStatusWebAutoMapperProfile : Profile
{
    public advPreStatusWebAutoMapperProfile()
    {
        CreateMap<advPreStatusDto, EditadvPreStatusViewModel>();
        CreateMap<CreateadvPreStatusViewModel, CreateUpdateadvPreStatusDto>();
        CreateMap<EditadvPreStatusViewModel, CreateUpdateadvPreStatusDto>();
    }
}
