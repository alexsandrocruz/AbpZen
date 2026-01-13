using AutoMapper;
using Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosDadosHerdeiros.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProcessosDadosHerdeirosWebAutoMapperProfile : Profile
{
    public advProcessosDadosHerdeirosWebAutoMapperProfile()
    {
        CreateMap<advProcessosDadosHerdeirosDto, EditadvProcessosDadosHerdeirosViewModel>();
        CreateMap<CreateadvProcessosDadosHerdeirosViewModel, CreateUpdateadvProcessosDadosHerdeirosDto>();
        CreateMap<EditadvProcessosDadosHerdeirosViewModel, CreateUpdateadvProcessosDadosHerdeirosDto>();
    }
}
