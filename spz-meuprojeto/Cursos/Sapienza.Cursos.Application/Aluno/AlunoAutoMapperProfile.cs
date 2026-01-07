using AutoMapper;
using Sapienza.Cursos.Aluno.Dtos;

namespace Sapienza.Cursos.Aluno;

public class AlunoAutoMapperProfile : Profile
{
    public AlunoAutoMapperProfile()
    {
        CreateMap<Aluno, AlunoDto>();
        CreateMap<CreateUpdateAlunoDto, Aluno>();
        CreateMap<CreateUpdateAlunoDto, Aluno>();
    }
}
