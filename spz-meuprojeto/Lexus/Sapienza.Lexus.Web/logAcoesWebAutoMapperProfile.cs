using AutoMapper;
using Sapienza.Lexus.logAcoes.Dtos;
using Sapienza.Lexus.Web.Pages.logAcoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class logAcoesWebAutoMapperProfile : Profile
{
    public logAcoesWebAutoMapperProfile()
    {
        CreateMap<logAcoesDto, EditlogAcoesViewModel>();
        CreateMap<CreatelogAcoesViewModel, CreateUpdatelogAcoesDto>();
        CreateMap<EditlogAcoesViewModel, CreateUpdatelogAcoesDto>();
    }
}
