using AutoMapper;
using LeptonXDemoApp.Edital.Dtos;
using LeptonXDemoApp.Web.Pages.Edital.ViewModels;

namespace LeptonXDemoApp.Web;

public class EditalWebAutoMapperProfile : Profile
{
    public EditalWebAutoMapperProfile()
    {
        CreateMap<EditalDto, EditEditalViewModel>();
        CreateMap<CreateEditalViewModel, CreateUpdateEditalDto>();
        CreateMap<EditEditalViewModel, CreateUpdateEditalDto>();
    }
}
