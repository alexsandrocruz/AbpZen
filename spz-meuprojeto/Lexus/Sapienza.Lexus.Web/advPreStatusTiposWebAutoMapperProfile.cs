using AutoMapper;
using Sapienza.Lexus.advPreStatusTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advPreStatusTipos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreStatusTiposWebAutoMapperProfile : Profile
{
    public advPreStatusTiposWebAutoMapperProfile()
    {
        CreateMap<advPreStatusTiposDto, EditadvPreStatusTiposViewModel>();
        CreateMap<CreateadvPreStatusTiposViewModel, CreateUpdateadvPreStatusTiposDto>();
        CreateMap<EditadvPreStatusTiposViewModel, CreateUpdateadvPreStatusTiposDto>();
    }
}
