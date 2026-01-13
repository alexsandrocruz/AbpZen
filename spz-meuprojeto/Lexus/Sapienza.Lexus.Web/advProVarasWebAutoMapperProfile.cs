using AutoMapper;
using Sapienza.Lexus.advProVaras.Dtos;
using Sapienza.Lexus.Web.Pages.advProVaras.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProVarasWebAutoMapperProfile : Profile
{
    public advProVarasWebAutoMapperProfile()
    {
        CreateMap<advProVarasDto, EditadvProVarasViewModel>();
        CreateMap<CreateadvProVarasViewModel, CreateUpdateadvProVarasDto>();
        CreateMap<EditadvProVarasViewModel, CreateUpdateadvProVarasDto>();
    }
}
