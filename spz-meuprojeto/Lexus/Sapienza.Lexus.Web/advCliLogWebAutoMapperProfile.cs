using AutoMapper;
using Sapienza.Lexus.advCliLog.Dtos;
using Sapienza.Lexus.Web.Pages.advCliLog.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliLogWebAutoMapperProfile : Profile
{
    public advCliLogWebAutoMapperProfile()
    {
        CreateMap<advCliLogDto, EditadvCliLogViewModel>();
        CreateMap<CreateadvCliLogViewModel, CreateUpdateadvCliLogDto>();
        CreateMap<EditadvCliLogViewModel, CreateUpdateadvCliLogDto>();
    }
}
