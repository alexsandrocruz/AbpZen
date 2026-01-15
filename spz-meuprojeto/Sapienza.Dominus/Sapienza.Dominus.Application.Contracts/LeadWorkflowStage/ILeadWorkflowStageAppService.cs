using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadWorkflowStage.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadWorkflowStage;

public interface ILeadWorkflowStageAppService :
    ICrudAppService<
        LeadWorkflowStageDto,
        Guid,
        LeadWorkflowStageGetListInput,
        CreateUpdateLeadWorkflowStageDto,
        CreateUpdateLeadWorkflowStageDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadWorkflowStageLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
