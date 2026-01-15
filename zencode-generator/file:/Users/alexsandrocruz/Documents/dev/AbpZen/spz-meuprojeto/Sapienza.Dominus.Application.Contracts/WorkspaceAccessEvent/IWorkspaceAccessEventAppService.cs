using System;
using System.Threading.Tasks;
using Sapienza.Dominus.WorkspaceAccessEvent.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.WorkspaceAccessEvent;

public interface IWorkspaceAccessEventAppService :
    ICrudAppService<
        WorkspaceAccessEventDto,
        Guid,
        WorkspaceAccessEventGetListInput,
        CreateUpdateWorkspaceAccessEventDto,
        CreateUpdateWorkspaceAccessEventDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetWorkspaceAccessEventLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
