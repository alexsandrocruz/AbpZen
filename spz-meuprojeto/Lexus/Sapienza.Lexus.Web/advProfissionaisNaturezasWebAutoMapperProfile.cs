using AutoMapper;
using Sapienza.Lexus.advProfissionaisNaturezas.Dtos;
using Sapienza.Lexus.Web.Pages.advProfissionaisNaturezas.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProfissionaisNaturezasWebAutoMapperProfile : Profile
{
    public advProfissionaisNaturezasWebAutoMapperProfile()
    {
        CreateMap<advProfissionaisNaturezasDto, EditadvProfissionaisNaturezasViewModel>();
        CreateMap<CreateadvProfissionaisNaturezasViewModel, CreateUpdateadvProfissionaisNaturezasDto>();
        CreateMap<EditadvProfissionaisNaturezasViewModel, CreateUpdateadvProfissionaisNaturezasDto>();
    }
}
