using System;
using System.Threading.Tasks;
using Sapienza.Dominus.WorkspaceUsageMetric.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.WorkspaceUsageMetric;

public interface IWorkspaceUsageMetricAppService :
    ICrudAppService<
        WorkspaceUsageMetricDto,
        Guid,
        WorkspaceUsageMetricGetListInput,
        CreateUpdateWorkspaceUsageMetricDto,
        CreateUpdateWorkspaceUsageMetricDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetWorkspaceUsageMetricLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
