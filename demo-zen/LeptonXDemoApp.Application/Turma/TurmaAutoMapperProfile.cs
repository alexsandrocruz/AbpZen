using AutoMapper;
using LeptonXDemoApp.Turma.Dtos;

namespace LeptonXDemoApp.Turma;

public class TurmaAutoMapperProfile : Profile
{
    public TurmaAutoMapperProfile()
    {
        CreateMap<Turma, TurmaDto>();
        CreateMap<CreateUpdateTurmaDto, Turma>();
        CreateMap<CreateUpdateTurmaDto, Turma>();
    }
}
