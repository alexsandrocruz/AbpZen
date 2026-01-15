using System;
using System.Threading.Tasks;
using Sapienza.Dominus.WorkspaceInvite.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.WorkspaceInvite;

public interface IWorkspaceInviteAppService :
    ICrudAppService<
        WorkspaceInviteDto,
        Guid,
        WorkspaceInviteGetListInput,
        CreateUpdateWorkspaceInviteDto,
        CreateUpdateWorkspaceInviteDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetWorkspaceInviteLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
