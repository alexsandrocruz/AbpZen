using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadAutomation.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadAutomation;

public interface ILeadAutomationAppService :
    ICrudAppService<
        LeadAutomationDto,
        Guid,
        LeadAutomationGetListInput,
        CreateUpdateLeadAutomationDto,
        CreateUpdateLeadAutomationDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadAutomationLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
