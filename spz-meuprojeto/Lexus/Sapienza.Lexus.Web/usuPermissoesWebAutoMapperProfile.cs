using AutoMapper;
using Sapienza.Lexus.usuPermissoes.Dtos;
using Sapienza.Lexus.Web.Pages.usuPermissoes.ViewModels;

namespace Sapienza.Lexus.Web;

public class usuPermissoesWebAutoMapperProfile : Profile
{
    public usuPermissoesWebAutoMapperProfile()
    {
        CreateMap<usuPermissoesDto, EditusuPermissoesViewModel>();
        CreateMap<CreateusuPermissoesViewModel, CreateUpdateusuPermissoesDto>();
        CreateMap<EditusuPermissoesViewModel, CreateUpdateusuPermissoesDto>();
    }
}
