using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Web.Pages.Identity.Components.IdentitySettingGroup;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

namespace Volo.Abp.Identity.Web.Settings;

public class IdentitySettingManagementPageContributor : SettingPageContributorBase
{
    public IdentitySettingManagementPageContributor()
    {
        RequiredPermissions(IdentityPermissions.SettingManagement);
    }

    public override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<IdentityResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.Identity",
                l["Menu:IdentityManagement"],
                typeof(IdentitySettingGroupViewComponent)
            )
        );
        return Task.CompletedTask;
    }
}
