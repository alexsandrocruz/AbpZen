using AutoMapper;
using Sapienza.Lexus.finUnidades.Dtos;
using Sapienza.Lexus.Web.Pages.finUnidades.ViewModels;

namespace Sapienza.Lexus.Web;

public class finUnidadesWebAutoMapperProfile : Profile
{
    public finUnidadesWebAutoMapperProfile()
    {
        CreateMap<finUnidadesDto, EditfinUnidadesViewModel>();
        CreateMap<CreatefinUnidadesViewModel, CreateUpdatefinUnidadesDto>();
        CreateMap<EditfinUnidadesViewModel, CreateUpdatefinUnidadesDto>();
    }
}
