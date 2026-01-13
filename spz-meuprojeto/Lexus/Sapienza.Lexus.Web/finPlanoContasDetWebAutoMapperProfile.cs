using AutoMapper;
using Sapienza.Lexus.finPlanoContasDet.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContasDet.ViewModels;

namespace Sapienza.Lexus.Web;

public class finPlanoContasDetWebAutoMapperProfile : Profile
{
    public finPlanoContasDetWebAutoMapperProfile()
    {
        CreateMap<finPlanoContasDetDto, EditfinPlanoContasDetViewModel>();
        CreateMap<CreatefinPlanoContasDetViewModel, CreateUpdatefinPlanoContasDetDto>();
        CreateMap<EditfinPlanoContasDetViewModel, CreateUpdatefinPlanoContasDetDto>();
    }
}
