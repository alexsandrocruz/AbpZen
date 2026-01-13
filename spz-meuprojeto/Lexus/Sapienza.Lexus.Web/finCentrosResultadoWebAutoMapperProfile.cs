using AutoMapper;
using Sapienza.Lexus.finCentrosResultado.Dtos;
using Sapienza.Lexus.Web.Pages.finCentrosResultado.ViewModels;

namespace Sapienza.Lexus.Web;

public class finCentrosResultadoWebAutoMapperProfile : Profile
{
    public finCentrosResultadoWebAutoMapperProfile()
    {
        CreateMap<finCentrosResultadoDto, EditfinCentrosResultadoViewModel>();
        CreateMap<CreatefinCentrosResultadoViewModel, CreateUpdatefinCentrosResultadoDto>();
        CreateMap<EditfinCentrosResultadoViewModel, CreateUpdatefinCentrosResultadoDto>();
    }
}
