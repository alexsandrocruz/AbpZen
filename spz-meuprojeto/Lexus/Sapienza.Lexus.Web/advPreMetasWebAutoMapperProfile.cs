using AutoMapper;
using Sapienza.Lexus.advPreMetas.Dtos;
using Sapienza.Lexus.Web.Pages.advPreMetas.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreMetasWebAutoMapperProfile : Profile
{
    public advPreMetasWebAutoMapperProfile()
    {
        CreateMap<advPreMetasDto, EditadvPreMetasViewModel>();
        CreateMap<CreateadvPreMetasViewModel, CreateUpdateadvPreMetasDto>();
        CreateMap<EditadvPreMetasViewModel, CreateUpdateadvPreMetasDto>();
    }
}
