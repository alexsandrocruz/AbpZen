using AutoMapper;
using Sapienza.Lexus.finPlanoContasGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContasGrupos.ViewModels;

namespace Sapienza.Lexus.Web;

public class finPlanoContasGruposWebAutoMapperProfile : Profile
{
    public finPlanoContasGruposWebAutoMapperProfile()
    {
        CreateMap<finPlanoContasGruposDto, EditfinPlanoContasGruposViewModel>();
        CreateMap<CreatefinPlanoContasGruposViewModel, CreateUpdatefinPlanoContasGruposDto>();
        CreateMap<EditfinPlanoContasGruposViewModel, CreateUpdatefinPlanoContasGruposDto>();
    }
}
