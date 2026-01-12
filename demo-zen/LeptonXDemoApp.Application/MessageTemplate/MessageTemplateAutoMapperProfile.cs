using AutoMapper;
using LeptonXDemoApp.MessageTemplate.Dtos;

namespace LeptonXDemoApp.MessageTemplate;

public class MessageTemplateAutoMapperProfile : Profile
{
    public MessageTemplateAutoMapperProfile()
    {
        CreateMap<MessageTemplate, MessageTemplateDto>();
        CreateMap<CreateUpdateMessageTemplateDto, MessageTemplate>();
        CreateMap<CreateUpdateMessageTemplateDto, MessageTemplate>();
    }
}
