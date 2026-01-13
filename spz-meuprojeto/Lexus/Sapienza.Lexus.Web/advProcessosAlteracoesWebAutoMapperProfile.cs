using AutoMapper;
using Sapienza.Lexus.advProcessosAlteracoes.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosAlteracoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProcessosAlteracoesWebAutoMapperProfile : Profile
{
    public advProcessosAlteracoesWebAutoMapperProfile()
    {
        CreateMap<advProcessosAlteracoesDto, EditadvProcessosAlteracoesViewModel>();
        CreateMap<CreateadvProcessosAlteracoesViewModel, CreateUpdateadvProcessosAlteracoesDto>();
        CreateMap<EditadvProcessosAlteracoesViewModel, CreateUpdateadvProcessosAlteracoesDto>();
    }
}
