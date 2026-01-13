using AutoMapper;
using Sapienza.Lexus.usuDistancias.Dtos;
using Sapienza.Lexus.Web.Pages.usuDistancias.ViewModels;

namespace Sapienza.Lexus.Web;

public class usuDistanciasWebAutoMapperProfile : Profile
{
    public usuDistanciasWebAutoMapperProfile()
    {
        CreateMap<usuDistanciasDto, EditusuDistanciasViewModel>();
        CreateMap<CreateusuDistanciasViewModel, CreateUpdateusuDistanciasDto>();
        CreateMap<EditusuDistanciasViewModel, CreateUpdateusuDistanciasDto>();
    }
}
