using AutoMapper;
using Sapienza.Lexus.advPreLogStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advPreLogStatus.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreLogStatusWebAutoMapperProfile : Profile
{
    public advPreLogStatusWebAutoMapperProfile()
    {
        CreateMap<advPreLogStatusDto, EditadvPreLogStatusViewModel>();
        CreateMap<CreateadvPreLogStatusViewModel, CreateUpdateadvPreLogStatusDto>();
        CreateMap<EditadvPreLogStatusViewModel, CreateUpdateadvPreLogStatusDto>();
    }
}
