using AutoMapper;
using Sapienza.Dominus.AiChatMessage.Dtos;

namespace Sapienza.Dominus.AiChatMessage;

public class AiChatMessageAutoMapperProfile : Profile
{
    public AiChatMessageAutoMapperProfile()
    {
        CreateMap<AiChatMessage, AiChatMessageDto>()
            .ForMember(dest => dest.AiChatSessionDisplayName, opt => opt.MapFrom(src => src.AiChatSession.Title));
        CreateMap<CreateUpdateAiChatMessageDto, AiChatMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateAiChatMessageDto, AiChatMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
