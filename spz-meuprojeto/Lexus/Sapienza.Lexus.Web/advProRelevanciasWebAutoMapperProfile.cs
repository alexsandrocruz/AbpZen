using AutoMapper;
using Sapienza.Lexus.advProRelevancias.Dtos;
using Sapienza.Lexus.Web.Pages.advProRelevancias.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProRelevanciasWebAutoMapperProfile : Profile
{
    public advProRelevanciasWebAutoMapperProfile()
    {
        CreateMap<advProRelevanciasDto, EditadvProRelevanciasViewModel>();
        CreateMap<CreateadvProRelevanciasViewModel, CreateUpdateadvProRelevanciasDto>();
        CreateMap<EditadvProRelevanciasViewModel, CreateUpdateadvProRelevanciasDto>();
    }
}
