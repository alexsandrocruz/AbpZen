using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Account.Admin.Web.Pages.Account.Components.AccountSettingGroup;
using Volo.Abp.Account.Localization;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

namespace Volo.Abp.Account.Admin.Web.Settings;

public class AccountSettingManagementPageContributor : SettingPageContributorBase
{
    public AccountSettingManagementPageContributor()
    {
        RequiredPermissions(AccountPermissions.SettingManagement);
    }

    public override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AccountResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.Account",
                l["Menu:Account"],
                typeof(AccountSettingGroupViewComponent)
            )
        );
        
        return Task.CompletedTask;
    }
}
