using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AuditLogging.Web.Pages.AuditLogs.Components.AuditLogSettingGroup;

namespace Volo.Abp.AuditLogging.Web.Controllers;

[Route("api/audit-logging/settings-widgets")]
public class AuditLoggingSettingsWidgetController : AbpController
{
    [HttpGet]
    [Route("audit-log-setting-group")]
    public IActionResult GetAuditLogSettingGroup()
    {
        return ViewComponent(typeof(AuditLogSettingGroupViewComponent));
    }
}
