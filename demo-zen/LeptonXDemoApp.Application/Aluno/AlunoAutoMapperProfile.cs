using AutoMapper;
using LeptonXDemoApp.Aluno.Dtos;

namespace LeptonXDemoApp.Aluno;

public class AlunoAutoMapperProfile : Profile
{
    public AlunoAutoMapperProfile()
    {
        CreateMap<Aluno, AlunoDto>();
        CreateMap<CreateUpdateAlunoDto, Aluno>();
        CreateMap<CreateUpdateAlunoDto, Aluno>();
    }
}
