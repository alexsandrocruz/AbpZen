using AutoMapper;
using LeptonXDemoApp.LeadContact.Dtos;

namespace LeptonXDemoApp.LeadContact;

public class LeadContactAutoMapperProfile : Profile
{
    public LeadContactAutoMapperProfile()
    {
        CreateMap<LeadContact, LeadContactDto>()
            .ForMember(dest => dest.LeadDisplayName, opt => opt.MapFrom(src => src.Lead.Name));
        CreateMap<CreateUpdateLeadContactDto, LeadContact>();
        CreateMap<CreateUpdateLeadContactDto, LeadContact>();
    }
}
