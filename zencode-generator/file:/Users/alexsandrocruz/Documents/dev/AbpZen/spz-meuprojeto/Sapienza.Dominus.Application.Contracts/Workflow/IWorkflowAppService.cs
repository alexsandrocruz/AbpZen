using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Workflow.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Workflow;

public interface IWorkflowAppService :
    ICrudAppService<
        WorkflowDto,
        Guid,
        WorkflowGetListInput,
        CreateUpdateWorkflowDto,
        CreateUpdateWorkflowDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetWorkflowLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
