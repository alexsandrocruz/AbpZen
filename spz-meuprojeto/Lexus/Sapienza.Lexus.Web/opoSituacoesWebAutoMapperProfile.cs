using AutoMapper;
using Sapienza.Lexus.opoSituacoes.Dtos;
using Sapienza.Lexus.Web.Pages.opoSituacoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class opoSituacoesWebAutoMapperProfile : Profile
{
    public opoSituacoesWebAutoMapperProfile()
    {
        CreateMap<opoSituacoesDto, EditopoSituacoesViewModel>();
        CreateMap<CreateopoSituacoesViewModel, CreateUpdateopoSituacoesDto>();
        CreateMap<EditopoSituacoesViewModel, CreateUpdateopoSituacoesDto>();
    }
}
