using AutoMapper;
using LeptonXDemoApp.LeadMessage.Dtos;

namespace LeptonXDemoApp.LeadMessage;

public class LeadMessageAutoMapperProfile : Profile
{
    public LeadMessageAutoMapperProfile()
    {
        CreateMap<LeadMessage, LeadMessageDto>()
            .ForMember(dest => dest.MessageTemplateDisplayName, opt => opt.MapFrom(src => src.MessageTemplate.Title))
            .ForMember(dest => dest.LeadDisplayName, opt => opt.MapFrom(src => src.Lead.Name));
        CreateMap<CreateUpdateLeadMessageDto, LeadMessage>();
        CreateMap<CreateUpdateLeadMessageDto, LeadMessage>();
    }
}
