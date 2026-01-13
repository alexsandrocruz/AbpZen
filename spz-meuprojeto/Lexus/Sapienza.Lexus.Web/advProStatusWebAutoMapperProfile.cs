using AutoMapper;
using Sapienza.Lexus.advProStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advProStatus.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProStatusWebAutoMapperProfile : Profile
{
    public advProStatusWebAutoMapperProfile()
    {
        CreateMap<advProStatusDto, EditadvProStatusViewModel>();
        CreateMap<CreateadvProStatusViewModel, CreateUpdateadvProStatusDto>();
        CreateMap<EditadvProStatusViewModel, CreateUpdateadvProStatusDto>();
    }
}
