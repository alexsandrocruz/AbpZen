using AutoMapper;
using Sapienza.Lexus.flwGradeHorarios.Dtos;
using Sapienza.Lexus.Web.Pages.flwGradeHorarios.ViewModels;

namespace Sapienza.Lexus.Web;

public class flwGradeHorariosWebAutoMapperProfile : Profile
{
    public flwGradeHorariosWebAutoMapperProfile()
    {
        CreateMap<flwGradeHorariosDto, EditflwGradeHorariosViewModel>();
        CreateMap<CreateflwGradeHorariosViewModel, CreateUpdateflwGradeHorariosDto>();
        CreateMap<EditflwGradeHorariosViewModel, CreateUpdateflwGradeHorariosDto>();
    }
}
