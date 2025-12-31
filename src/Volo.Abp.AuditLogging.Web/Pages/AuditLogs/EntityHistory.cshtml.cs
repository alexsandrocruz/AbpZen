using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.AuditLogging.Web.Pages.AuditLogs;

public class EntityHistoryModel : AuditLogsPageModel
{
    public List<EntityChangeWithUsernameDto> EntityChanges { get; set; }

    [BindProperty(SupportsGet = true)]
    public string EntityId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string EntityTypeFullName { get; set; }

    protected IAuditLogsAppService AuditLogsAppService { get; }

    public EntityHistoryModel(IAuditLogsAppService auditLogsAppService)
    {
        AuditLogsAppService = auditLogsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        EntityChanges = await AuditLogsAppService.GetEntityChangesWithUsernameAsync(new EntityChangeFilter()
        { EntityId = EntityId, EntityTypeFullName = EntityTypeFullName });
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
