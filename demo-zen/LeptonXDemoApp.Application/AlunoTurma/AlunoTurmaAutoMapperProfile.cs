using AutoMapper;
using LeptonXDemoApp.AlunoTurma.Dtos;

namespace LeptonXDemoApp.AlunoTurma;

public class AlunoTurmaAutoMapperProfile : Profile
{
    public AlunoTurmaAutoMapperProfile()
    {
        CreateMap<AlunoTurma, AlunoTurmaDto>()
            .ForMember(dest => dest.AlunoDisplayName, opt => opt.MapFrom(src => src.Aluno.Nome))
            .ForMember(dest => dest.TurmaDisplayName, opt => opt.MapFrom(src => src.Turma.Nome));
        CreateMap<CreateUpdateAlunoTurmaDto, AlunoTurma>();
        CreateMap<CreateUpdateAlunoTurmaDto, AlunoTurma>();
    }
}
