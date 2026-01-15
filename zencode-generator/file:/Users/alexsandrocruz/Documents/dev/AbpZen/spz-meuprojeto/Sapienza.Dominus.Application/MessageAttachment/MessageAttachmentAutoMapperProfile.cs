using AutoMapper;
using Sapienza.Dominus.MessageAttachment.Dtos;

namespace Sapienza.Dominus.MessageAttachment;

public class MessageAttachmentAutoMapperProfile : Profile
{
    public MessageAttachmentAutoMapperProfile()
    {
        CreateMap<MessageAttachment, MessageAttachmentDto>()
            .ForMember(dest => dest.ChatMessageDisplayName, opt => opt.MapFrom(src => src.ChatMessage.Content));
        CreateMap<CreateUpdateMessageAttachmentDto, MessageAttachment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateMessageAttachmentDto, MessageAttachment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
