using AutoMapper;
using Sapienza.Cursos.Turma.Dtos;

namespace Sapienza.Cursos.Turma;

public class TurmaAutoMapperProfile : Profile
{
    public TurmaAutoMapperProfile()
    {
        CreateMap<Turma, TurmaDto>();
        CreateMap<CreateUpdateTurmaDto, Turma>();
        CreateMap<CreateUpdateTurmaDto, Turma>();
    }
}
