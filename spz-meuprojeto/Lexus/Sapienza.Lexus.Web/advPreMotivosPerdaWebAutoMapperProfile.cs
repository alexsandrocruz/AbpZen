using AutoMapper;
using Sapienza.Lexus.advPreMotivosPerda.Dtos;
using Sapienza.Lexus.Web.Pages.advPreMotivosPerda.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreMotivosPerdaWebAutoMapperProfile : Profile
{
    public advPreMotivosPerdaWebAutoMapperProfile()
    {
        CreateMap<advPreMotivosPerdaDto, EditadvPreMotivosPerdaViewModel>();
        CreateMap<CreateadvPreMotivosPerdaViewModel, CreateUpdateadvPreMotivosPerdaDto>();
        CreateMap<EditadvPreMotivosPerdaViewModel, CreateUpdateadvPreMotivosPerdaDto>();
    }
}
