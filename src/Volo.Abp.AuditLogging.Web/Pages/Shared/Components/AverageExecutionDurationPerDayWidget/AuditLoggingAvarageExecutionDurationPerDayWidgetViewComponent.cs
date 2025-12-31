using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AuditLogging.Web.Pages.Shared.Components.AverageExecutionDurationPerDayWidget;

[Widget(
    RequiredPolicies = new[] { AbpAuditLoggingPermissions.AuditLogs.Default },
    ScriptTypes = new[] { typeof(AuditLoggingAverageExecutionDurationPerDayWidgetScriptContributor) }
    )]
public class AuditLoggingAverageExecutionDurationPerDayWidgetViewComponent : AuditLogsComponentBase
{
    public virtual IViewComponentResult Invoke()
    {
        return View("/Pages/Shared/Components/AverageExecutionDurationPerDayWidget/Default.cshtml");
    }
}
