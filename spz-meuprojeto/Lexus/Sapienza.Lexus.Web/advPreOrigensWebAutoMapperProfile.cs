using AutoMapper;
using Sapienza.Lexus.advPreOrigens.Dtos;
using Sapienza.Lexus.Web.Pages.advPreOrigens.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreOrigensWebAutoMapperProfile : Profile
{
    public advPreOrigensWebAutoMapperProfile()
    {
        CreateMap<advPreOrigensDto, EditadvPreOrigensViewModel>();
        CreateMap<CreateadvPreOrigensViewModel, CreateUpdateadvPreOrigensDto>();
        CreateMap<EditadvPreOrigensViewModel, CreateUpdateadvPreOrigensDto>();
    }
}
