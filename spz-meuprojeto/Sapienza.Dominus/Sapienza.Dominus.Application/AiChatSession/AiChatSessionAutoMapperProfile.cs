using AutoMapper;
using Sapienza.Dominus.AiChatSession.Dtos;

namespace Sapienza.Dominus.AiChatSession;

public class AiChatSessionAutoMapperProfile : Profile
{
    public AiChatSessionAutoMapperProfile()
    {
        CreateMap<AiChatSession, AiChatSessionDto>();
        CreateMap<CreateUpdateAiChatSessionDto, AiChatSession>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateAiChatSessionDto, AiChatSession>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
