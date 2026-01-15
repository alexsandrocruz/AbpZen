using AutoMapper;
using Sapienza.Dominus.LeadScheduledMessage.Dtos;

namespace Sapienza.Dominus.LeadScheduledMessage;

public class LeadScheduledMessageAutoMapperProfile : Profile
{
    public LeadScheduledMessageAutoMapperProfile()
    {
        CreateMap<LeadScheduledMessage, LeadScheduledMessageDto>()
            .ForMember(dest => dest.LeadAutomationDisplayName, opt => opt.MapFrom(src => src.LeadAutomation.Name));
        CreateMap<CreateUpdateLeadScheduledMessageDto, LeadScheduledMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadScheduledMessageDto, LeadScheduledMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
