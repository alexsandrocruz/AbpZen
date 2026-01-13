using AutoMapper;
using Sapienza.Lexus.advProNaturezas.Dtos;
using Sapienza.Lexus.Web.Pages.advProNaturezas.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProNaturezasWebAutoMapperProfile : Profile
{
    public advProNaturezasWebAutoMapperProfile()
    {
        CreateMap<advProNaturezasDto, EditadvProNaturezasViewModel>();
        CreateMap<CreateadvProNaturezasViewModel, CreateUpdateadvProNaturezasDto>();
        CreateMap<EditadvProNaturezasViewModel, CreateUpdateadvProNaturezasDto>();
    }
}
