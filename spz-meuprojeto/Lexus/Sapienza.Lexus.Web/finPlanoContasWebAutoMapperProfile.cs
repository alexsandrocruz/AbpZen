using AutoMapper;
using Sapienza.Lexus.finPlanoContas.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContas.ViewModels;

namespace Sapienza.Lexus.Web;

public class finPlanoContasWebAutoMapperProfile : Profile
{
    public finPlanoContasWebAutoMapperProfile()
    {
        CreateMap<finPlanoContasDto, EditfinPlanoContasViewModel>();
        CreateMap<CreatefinPlanoContasViewModel, CreateUpdatefinPlanoContasDto>();
        CreateMap<EditfinPlanoContasViewModel, CreateUpdatefinPlanoContasDto>();
    }
}
