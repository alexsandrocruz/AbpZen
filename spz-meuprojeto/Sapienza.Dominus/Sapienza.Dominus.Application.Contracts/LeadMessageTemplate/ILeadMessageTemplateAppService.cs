using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadMessageTemplate.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadMessageTemplate;

public interface ILeadMessageTemplateAppService :
    ICrudAppService<
        LeadMessageTemplateDto,
        Guid,
        LeadMessageTemplateGetListInput,
        CreateUpdateLeadMessageTemplateDto,
        CreateUpdateLeadMessageTemplateDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadMessageTemplateLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
