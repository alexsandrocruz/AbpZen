using AutoMapper;
using Sapienza.Dominus.Conversation.Dtos;

namespace Sapienza.Dominus.Conversation;

public class ConversationAutoMapperProfile : Profile
{
    public ConversationAutoMapperProfile()
    {
        CreateMap<Conversation, ConversationDto>()
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name));
        CreateMap<CreateUpdateConversationDto, Conversation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateConversationDto, Conversation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
