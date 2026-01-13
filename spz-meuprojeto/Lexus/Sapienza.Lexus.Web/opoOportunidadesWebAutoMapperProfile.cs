using AutoMapper;
using Sapienza.Lexus.opoOportunidades.Dtos;
using Sapienza.Lexus.Web.Pages.opoOportunidades.ViewModels;

namespace Sapienza.Lexus.Web;

public class opoOportunidadesWebAutoMapperProfile : Profile
{
    public opoOportunidadesWebAutoMapperProfile()
    {
        CreateMap<opoOportunidadesDto, EditopoOportunidadesViewModel>();
        CreateMap<CreateopoOportunidadesViewModel, CreateUpdateopoOportunidadesDto>();
        CreateMap<EditopoOportunidadesViewModel, CreateUpdateopoOportunidadesDto>();
    }
}
