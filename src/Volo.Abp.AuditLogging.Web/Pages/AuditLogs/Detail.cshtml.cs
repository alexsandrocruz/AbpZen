using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Volo.Abp.AuditLogging.Web.Pages.AuditLogs;

public class DetailModel : AuditLogsPageModel
{
    public AuditLogDto AuditLog { get; private set; }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    protected IAuditLogsAppService AuditLogsAppService { get; }

    public DetailModel(IAuditLogsAppService auditLogsAppService)
    {
        AuditLogsAppService = auditLogsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        AuditLog = await AuditLogsAppService.GetAsync(Id);
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual string SerializeDictionary(Dictionary<string, object> dictionary)
    {
        return JsonSerializer.Serialize(dictionary, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
    }
}
