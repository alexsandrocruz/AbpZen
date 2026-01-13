using AutoMapper;
using Sapienza.Lexus.advVerbas.Dtos;
using Sapienza.Lexus.Web.Pages.advVerbas.ViewModels;

namespace Sapienza.Lexus.Web;

public class advVerbasWebAutoMapperProfile : Profile
{
    public advVerbasWebAutoMapperProfile()
    {
        CreateMap<advVerbasDto, EditadvVerbasViewModel>();
        CreateMap<CreateadvVerbasViewModel, CreateUpdateadvVerbasDto>();
        CreateMap<EditadvVerbasViewModel, CreateUpdateadvVerbasDto>();
    }
}
