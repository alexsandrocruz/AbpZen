using AutoMapper;
using Sapienza.Lexus.advProcessosHonorarios.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosHonorarios.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProcessosHonorariosWebAutoMapperProfile : Profile
{
    public advProcessosHonorariosWebAutoMapperProfile()
    {
        CreateMap<advProcessosHonorariosDto, EditadvProcessosHonorariosViewModel>();
        CreateMap<CreateadvProcessosHonorariosViewModel, CreateUpdateadvProcessosHonorariosDto>();
        CreateMap<EditadvProcessosHonorariosViewModel, CreateUpdateadvProcessosHonorariosDto>();
    }
}
