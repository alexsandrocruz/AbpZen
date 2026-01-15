using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadWorkflow.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadWorkflow;

public interface ILeadWorkflowAppService :
    ICrudAppService<
        LeadWorkflowDto,
        Guid,
        LeadWorkflowGetListInput,
        CreateUpdateLeadWorkflowDto,
        CreateUpdateLeadWorkflowDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadWorkflowLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
