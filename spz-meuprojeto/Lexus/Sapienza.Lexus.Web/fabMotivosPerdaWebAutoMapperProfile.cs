using AutoMapper;
using Sapienza.Lexus.fabMotivosPerda.Dtos;
using Sapienza.Lexus.Web.Pages.fabMotivosPerda.ViewModels;

namespace Sapienza.Lexus.Web;

public class fabMotivosPerdaWebAutoMapperProfile : Profile
{
    public fabMotivosPerdaWebAutoMapperProfile()
    {
        CreateMap<fabMotivosPerdaDto, EditfabMotivosPerdaViewModel>();
        CreateMap<CreatefabMotivosPerdaViewModel, CreateUpdatefabMotivosPerdaDto>();
        CreateMap<EditfabMotivosPerdaViewModel, CreateUpdatefabMotivosPerdaDto>();
    }
}
