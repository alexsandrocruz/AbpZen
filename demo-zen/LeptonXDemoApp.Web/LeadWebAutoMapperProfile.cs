using AutoMapper;
using LeptonXDemoApp.Lead.Dtos;
using LeptonXDemoApp.Web.Pages.Lead.ViewModels;

namespace LeptonXDemoApp.Web;

public class LeadWebAutoMapperProfile : Profile
{
    public LeadWebAutoMapperProfile()
    {
        CreateMap<LeadDto, EditLeadViewModel>();
        CreateMap<CreateLeadViewModel, CreateUpdateLeadDto>();
        CreateMap<EditLeadViewModel, CreateUpdateLeadDto>();
    }
}
