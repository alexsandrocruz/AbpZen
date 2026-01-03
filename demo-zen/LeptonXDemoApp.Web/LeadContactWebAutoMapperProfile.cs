using AutoMapper;
using LeptonXDemoApp.LeadContact.Dtos;
using LeptonXDemoApp.Web.Pages.LeadContact.ViewModels;

namespace LeptonXDemoApp.Web;

public class LeadContactWebAutoMapperProfile : Profile
{
    public LeadContactWebAutoMapperProfile()
    {
        CreateMap<LeadContactDto, EditLeadContactViewModel>();
        CreateMap<CreateLeadContactViewModel, CreateUpdateLeadContactDto>();
        CreateMap<EditLeadContactViewModel, CreateUpdateLeadContactDto>();
    }
}
