using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging.Web.Pages.AuditLogs.Components.AuditLogSettingGroup;

[Widget(
    RefreshUrl = "/api/audit-logging/settings-widgets/audit-log-setting-group")]
public class AuditLogSettingGroupViewComponent  : AbpViewComponent
{
    public AuditLogSettingsViewModel SettingsViewModel { get; set; }

    protected IAuditLogSettingsAppService AuditLogSettingsAppService { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public AuditLogSettingGroupViewComponent(
        IAuditLogSettingsAppService auditLogSettingsAppService,
        ICurrentTenant currentTenant)
    {
        AuditLogSettingsAppService = auditLogSettingsAppService;
        CurrentTenant = currentTenant;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        SettingsViewModel = new AuditLogSettingsViewModel
        {
            AuditLogSettings = await AuditLogSettingsAppService.GetAsync(),
        };

        if (!CurrentTenant.IsAvailable)
        {
            SettingsViewModel.AuditLogGlobalSettings = await AuditLogSettingsAppService.GetGlobalAsync();
        }

        return View("~/Pages/AuditLogs/Components/AuditLogSettingGroup/Default.cshtml", SettingsViewModel);
    }

    public class AuditLogSettingsViewModel
    {
        public AuditLogSettingsDto AuditLogSettings { get; set; }

        public AuditLogGlobalSettingsDto AuditLogGlobalSettings { get; set; }
    }
}