using AutoMapper;
using LeptonXDemoApp.LeadMessage.Dtos;
using LeptonXDemoApp.Web.Pages.LeadMessage.ViewModels;

namespace LeptonXDemoApp.Web;

public class LeadMessageWebAutoMapperProfile : Profile
{
    public LeadMessageWebAutoMapperProfile()
    {
        CreateMap<LeadMessageDto, EditLeadMessageViewModel>();
        CreateMap<CreateLeadMessageViewModel, CreateUpdateLeadMessageDto>();
        CreateMap<EditLeadMessageViewModel, CreateUpdateLeadMessageDto>();
    }
}
