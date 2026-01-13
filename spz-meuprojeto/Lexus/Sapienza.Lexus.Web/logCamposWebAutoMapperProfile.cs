using AutoMapper;
using Sapienza.Lexus.logCampos.Dtos;
using Sapienza.Lexus.Web.Pages.logCampos.ViewModels;

namespace Sapienza.Lexus.Web;

public class logCamposWebAutoMapperProfile : Profile
{
    public logCamposWebAutoMapperProfile()
    {
        CreateMap<logCamposDto, EditlogCamposViewModel>();
        CreateMap<CreatelogCamposViewModel, CreateUpdatelogCamposDto>();
        CreateMap<EditlogCamposViewModel, CreateUpdatelogCamposDto>();
    }
}
