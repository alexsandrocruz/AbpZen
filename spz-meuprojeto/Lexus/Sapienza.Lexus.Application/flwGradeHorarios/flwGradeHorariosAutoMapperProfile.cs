using AutoMapper;
using Sapienza.Lexus.flwGradeHorarios.Dtos;

namespace Sapienza.Lexus.flwGradeHorarios;

public class flwGradeHorariosAutoMapperProfile : Profile
{
    public flwGradeHorariosAutoMapperProfile()
    {
        CreateMap<flwGradeHorarios, flwGradeHorariosDto>();
        CreateMap<CreateUpdateflwGradeHorariosDto, flwGradeHorarios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateflwGradeHorariosDto, flwGradeHorarios>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
