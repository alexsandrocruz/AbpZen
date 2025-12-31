using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.AuditLogging.Web.Pages.AuditLogs.Components.AuditLogSettingGroup;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

namespace Volo.Abp.AuditLogging.Web.Settings;

public class AuditLogSettingManagementPageContributor : SettingPageContributorBase
{
    public AuditLogSettingManagementPageContributor()
    {
        RequiredPermissions(AbpAuditLoggingPermissions.AuditLogs.SettingManagement);
    }

    public override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AuditLoggingResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.AuditLogging",
                l["Menu:AuditLogging"],
                typeof(AuditLogSettingGroupViewComponent)
            )
        );

        return Task.CompletedTask;
    }
}