using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Gdpr.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Gdpr.Web.Navigation;

public class AbpGdprWebMainMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.User)
        {
            var l = context.GetLocalizer<AbpGdprResource>();

            context.Menu.Items
                .AddIfNotContains(new ApplicationMenuItem(
                        GdprMenuNames.PersonalData, 
                        displayName: l["Menu:PersonalData"], 
                        "/Gdpr/PersonalData",
                        icon: "fa fa-lock")
                    .RequireAuthenticated()
                );
        }

        return Task.CompletedTask;
    }
}