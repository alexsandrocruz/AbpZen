using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AuditLogging.Blazor.Pages.AuditLogging.Components;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.SettingManagement.Blazor;

namespace Volo.Abp.AuditLogging.Blazor.Settings;
public class AuditLogPageContributor : ISettingComponentContributor
{
    public async Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context)
    {
        var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();
        return await authorizationService.IsGrantedAsync(AbpAuditLoggingPermissions.AuditLogs.SettingManagement);
    }

    public async Task ConfigureAsync(SettingComponentCreationContext context)
    {
        if (!await CheckPermissionsAsync(context))
        {
            return;
        }

        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AuditLoggingResource>>();
        context.Groups.Add(new SettingComponentGroup(
            "Volo.Abp.AuditLogging",
            l["Menu:AuditLogging"],
            typeof(AuditLogSettingGroup)
            ));
    }
}
