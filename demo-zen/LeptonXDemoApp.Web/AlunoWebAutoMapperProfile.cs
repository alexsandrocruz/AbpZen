using AutoMapper;
using LeptonXDemoApp.Aluno.Dtos;
using LeptonXDemoApp.Web.Pages.Aluno.ViewModels;

namespace LeptonXDemoApp.Web;

public class AlunoWebAutoMapperProfile : Profile
{
    public AlunoWebAutoMapperProfile()
    {
        CreateMap<AlunoDto, EditAlunoViewModel>();
        CreateMap<CreateAlunoViewModel, CreateUpdateAlunoDto>();
        CreateMap<EditAlunoViewModel, CreateUpdateAlunoDto>();
    }
}
