using AutoMapper;
using Sapienza.Lexus.advProFases.Dtos;
using Sapienza.Lexus.Web.Pages.advProFases.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProFasesWebAutoMapperProfile : Profile
{
    public advProFasesWebAutoMapperProfile()
    {
        CreateMap<advProFasesDto, EditadvProFasesViewModel>();
        CreateMap<CreateadvProFasesViewModel, CreateUpdateadvProFasesDto>();
        CreateMap<EditadvProFasesViewModel, CreateUpdateadvProFasesDto>();
    }
}
