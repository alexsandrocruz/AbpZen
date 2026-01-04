using AutoMapper;
using LeptonXDemoApp.Turma.Dtos;
using LeptonXDemoApp.Web.Pages.Turma.ViewModels;

namespace LeptonXDemoApp.Web;

public class TurmaWebAutoMapperProfile : Profile
{
    public TurmaWebAutoMapperProfile()
    {
        CreateMap<TurmaDto, EditTurmaViewModel>();
        CreateMap<CreateTurmaViewModel, CreateUpdateTurmaDto>();
        CreateMap<EditTurmaViewModel, CreateUpdateTurmaDto>();
    }
}
