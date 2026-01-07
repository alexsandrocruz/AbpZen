using AutoMapper;
using Sapienza.Cursos.Curso.Dtos;

namespace Sapienza.Cursos.Curso;

public class CursoAutoMapperProfile : Profile
{
    public CursoAutoMapperProfile()
    {
        CreateMap<Curso, CursoDto>();
        CreateMap<CreateUpdateCursoDto, Curso>();
        CreateMap<CreateUpdateCursoDto, Curso>();
    }
}
