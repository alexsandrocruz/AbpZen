using AutoMapper;
using Sapienza.Lexus.advProInstancias.Dtos;
using Sapienza.Lexus.Web.Pages.advProInstancias.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProInstanciasWebAutoMapperProfile : Profile
{
    public advProInstanciasWebAutoMapperProfile()
    {
        CreateMap<advProInstanciasDto, EditadvProInstanciasViewModel>();
        CreateMap<CreateadvProInstanciasViewModel, CreateUpdateadvProInstanciasDto>();
        CreateMap<EditadvProInstanciasViewModel, CreateUpdateadvProInstanciasDto>();
    }
}
