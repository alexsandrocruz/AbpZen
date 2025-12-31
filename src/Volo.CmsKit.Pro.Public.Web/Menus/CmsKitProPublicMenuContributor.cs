using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Public.Web.Menus;

public class CmsKitProPublicMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }
    
    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.

        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CmsKitResource>();
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

        if (GlobalFeatureManager.Instance.IsEnabled<NewslettersFeature>() && currentUser.IsAuthenticated)
        {
            context.Menu.AddItem(new ApplicationMenuItem(
                    CmsKitProPublicMenus.Newsletters.PreferencesMenu,
                    l["EmailPreferences"].Value,
                    $"/cms/newsletter/email-preferences",
                    "fas fa-mail-bulk"
                )
            );
        }

        return Task.CompletedTask;
    }
}
