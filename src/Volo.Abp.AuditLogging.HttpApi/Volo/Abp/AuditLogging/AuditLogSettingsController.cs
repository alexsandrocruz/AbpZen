using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.AuditLogging;

[RemoteService(Name = AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[Area(AuditLoggingRemoteServiceConsts.ModuleName)]
[ControllerName("Settings")]
[Route("api/audit-logging/settings")]
public class AuditLogSettingsController : AbpControllerBase, IAuditLogSettingsAppService
{
    protected IAuditLogSettingsAppService AuditLogSettingsAppService { get; }

    public AuditLogSettingsController(IAuditLogSettingsAppService auditLogSettingsAppService)
    {
        AuditLogSettingsAppService = auditLogSettingsAppService;
    }

    [HttpGet]
    public Task<AuditLogSettingsDto> GetAsync()
    {
        return AuditLogSettingsAppService.GetAsync();
    }

    [HttpPut]
    public Task UpdateAsync(AuditLogSettingsDto input)
    {
        return AuditLogSettingsAppService.UpdateAsync(input);
    }

    [HttpGet]
    [Route("global")]
    public Task<AuditLogGlobalSettingsDto> GetGlobalAsync()
    {
        return AuditLogSettingsAppService.GetGlobalAsync();
    }

    [HttpPut]
    [Route("global")]
    public Task UpdateGlobalAsync(AuditLogGlobalSettingsDto input)
    {
        return AuditLogSettingsAppService.UpdateGlobalAsync(input);
    }
}