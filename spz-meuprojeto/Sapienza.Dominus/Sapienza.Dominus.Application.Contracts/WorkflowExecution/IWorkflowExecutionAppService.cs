using System;
using System.Threading.Tasks;
using Sapienza.Dominus.WorkflowExecution.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.WorkflowExecution;

public interface IWorkflowExecutionAppService :
    ICrudAppService<
        WorkflowExecutionDto,
        Guid,
        WorkflowExecutionGetListInput,
        CreateUpdateWorkflowExecutionDto,
        CreateUpdateWorkflowExecutionDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetWorkflowExecutionLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
