using System;
using System.Threading.Tasks;
using LeptonXDemoApp.MessageTemplate.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.MessageTemplate;

public interface IMessageTemplateAppService :
    ICrudAppService<
        MessageTemplateDto,
        Guid,
        MessageTemplateGetListInput,
        CreateUpdateMessageTemplateDto,
        CreateUpdateMessageTemplateDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetMessageTemplateLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
