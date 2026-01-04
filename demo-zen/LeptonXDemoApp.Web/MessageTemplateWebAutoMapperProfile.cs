using AutoMapper;
using LeptonXDemoApp.MessageTemplate.Dtos;
using LeptonXDemoApp.Web.Pages.MessageTemplate.ViewModels;

namespace LeptonXDemoApp.Web;

public class MessageTemplateWebAutoMapperProfile : Profile
{
    public MessageTemplateWebAutoMapperProfile()
    {
        CreateMap<MessageTemplateDto, EditMessageTemplateViewModel>();
        CreateMap<CreateMessageTemplateViewModel, CreateUpdateMessageTemplateDto>();
        CreateMap<EditMessageTemplateViewModel, CreateUpdateMessageTemplateDto>();
    }
}
