using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.AuthorityDelegation;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Pro.Public.Blazor.Shared.Pages.Account.AuthorityDelegation;
using Volo.Abp.Account.Pro.Public.Blazor.Shared.Pages.Account.LinkUsers;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace Volo.Abp.Account.Pro.Public.Blazor.Server;

public class AccountBlazorUserMenuContributor: IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.User)
        {
            return Task.CompletedTask;
        }
        
        ConfigureUserMenus(context);
        
        context.Menu.GetMenuItemOrNull("Account.AuthorityDelegation")?.UseComponent<AuthorityDelegationModal>();

        context.Menu.GetMenuItem("Account.LinkedAccounts").UseComponent<LinkUsersModal>();
 
        
        return Task.CompletedTask;
    }

    private void ConfigureUserMenus(MenuConfigurationContext context)
    {
        var accountResource = context.GetLocalizer<AccountResource>();
        
        if (context.Menu.GetMenuItemOrNull("Account.LinkedAccounts") == null)
        {
            context.Menu.AddItem(new ApplicationMenuItem("Account.LinkedAccounts", accountResource["LinkedAccounts"], url: "javascript:void(0)", icon: "fa fa-link"));
        }
        
        if(context.Menu.GetMenuItemOrNull("Account.AuthorityDelegation") == null)
        {
             var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
             var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAccountAuthorityDelegationOptions>>();
            if (currentUser.FindImpersonatorUserId() == null && options.Value.EnableDelegatedImpersonation)
            {
                context.Menu.AddItem(new ApplicationMenuItem("Account.AuthorityDelegation", accountResource["AuthorityDelegation"], url: "javascript:void(0)", icon: "fa fa-users"));
            }
        }
    }
}