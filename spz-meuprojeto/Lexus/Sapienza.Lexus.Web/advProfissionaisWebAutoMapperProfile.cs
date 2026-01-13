using AutoMapper;
using Sapienza.Lexus.advProfissionais.Dtos;
using Sapienza.Lexus.Web.Pages.advProfissionais.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProfissionaisWebAutoMapperProfile : Profile
{
    public advProfissionaisWebAutoMapperProfile()
    {
        CreateMap<advProfissionaisDto, EditadvProfissionaisViewModel>();
        CreateMap<CreateadvProfissionaisViewModel, CreateUpdateadvProfissionaisDto>();
        CreateMap<EditadvProfissionaisViewModel, CreateUpdateadvProfissionaisDto>();
    }
}
