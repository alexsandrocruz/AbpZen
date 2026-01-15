using AutoMapper;
using Sapienza.Dominus.ChatMessage.Dtos;

namespace Sapienza.Dominus.ChatMessage;

public class ChatMessageAutoMapperProfile : Profile
{
    public ChatMessageAutoMapperProfile()
    {
        CreateMap<ChatMessage, ChatMessageDto>()
            .ForMember(dest => dest.ConversationDisplayName, opt => opt.MapFrom(src => src.Conversation.Name));
        CreateMap<CreateUpdateChatMessageDto, ChatMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateChatMessageDto, ChatMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
