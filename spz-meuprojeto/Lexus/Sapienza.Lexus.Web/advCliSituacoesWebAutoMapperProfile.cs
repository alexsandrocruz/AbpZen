using AutoMapper;
using Sapienza.Lexus.advCliSituacoes.Dtos;
using Sapienza.Lexus.Web.Pages.advCliSituacoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliSituacoesWebAutoMapperProfile : Profile
{
    public advCliSituacoesWebAutoMapperProfile()
    {
        CreateMap<advCliSituacoesDto, EditadvCliSituacoesViewModel>();
        CreateMap<CreateadvCliSituacoesViewModel, CreateUpdateadvCliSituacoesDto>();
        CreateMap<EditadvCliSituacoesViewModel, CreateUpdateadvCliSituacoesDto>();
    }
}
