using AutoMapper;
using LeptonXDemoApp.AlunoTurma.Dtos;
using LeptonXDemoApp.Web.Pages.AlunoTurma.ViewModels;

namespace LeptonXDemoApp.Web;

public class AlunoTurmaWebAutoMapperProfile : Profile
{
    public AlunoTurmaWebAutoMapperProfile()
    {
        CreateMap<AlunoTurmaDto, EditAlunoTurmaViewModel>();
        CreateMap<CreateAlunoTurmaViewModel, CreateUpdateAlunoTurmaDto>();
        CreateMap<EditAlunoTurmaViewModel, CreateUpdateAlunoTurmaDto>();
    }
}
