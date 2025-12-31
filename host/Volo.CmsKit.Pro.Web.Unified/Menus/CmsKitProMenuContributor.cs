using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Menus;

public class CmsKitProMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        await AddContactToMainMenuAsync(context);
        await AddNewsletterToMainMenuAsync(context);
        await AddPollToMainMenuAsync(context);
        await AddPageFeedbackToMainMenuAsync(context);
    }

    private Task AddContactToMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CmsKitResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
                CmsKitProMenus.Contact.Default,
                "Contact",
                "/Contact/Index",
                icon: "far fa-address-card"
            )
        );

        return Task.CompletedTask;
    }

    private Task AddNewsletterToMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CmsKitResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
                CmsKitProMenus.Newsletter.Default,
                "Newsletter",
                "/Newsletter/Index",
                icon: "fas fa-newspaper"
            )
        );

        return Task.CompletedTask;
    }

    private Task AddPollToMainMenuAsync(MenuConfigurationContext context)
    {
        context.Menu.AddItem(new ApplicationMenuItem(
                CmsKitProMenus.Poll.Default,
                "Poll",
                "/Poll/Index",
                icon: "fas fa-poll"
            )
        );

        return Task.CompletedTask;
    }
    
    private Task AddPageFeedbackToMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CmsKitResource>();
        context.Menu.AddItem(new ApplicationMenuItem(
                CmsKitProMenus.PageFeedback.Default,
                l["PageFeedback"],
                "/PageFeedback/Index",
                icon: "fas fa-comment"
            )
        );

        return Task.CompletedTask;
    }
}
