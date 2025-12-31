using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Pro.Admin.Web.Menus;

public class CmsKitProAdminMenuContributor : IMenuContributor
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
        await AddCmsMenuAsync(context);
    }

    private Task AddCmsMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CmsKitResource>();

        var cmsProMenus = new List<ApplicationMenuItem>();

        if (GlobalFeatureManager.Instance.IsEnabled<NewslettersFeature>())
        {
            cmsProMenus.Add(new ApplicationMenuItem(
                    CmsKitProAdminMenus.Newsletters.NewsletterMenu,
                    l["NewsletterUsers"].Value,
                    "/CmsKit/Newsletters",
                    "fas fa-newspaper"
                ).RequirePermissions(CmsKitProAdminPermissions.Newsletters.Default)
            );
        }
        
        if (GlobalFeatureManager.Instance.IsEnabled<PageFeedbackFeature>())
        {
            cmsProMenus.Add(new ApplicationMenuItem(
                    CmsKitProAdminMenus.PageFeedbacks.PageFeedbackMenu,
                    l["PageFeedbacks"].Value,
                    "/Cms/PageFeedbacks",
                    "fas fa-comment"
                ).RequirePermissions(CmsKitProAdminPermissions.PageFeedbacks.Default)
            );
        }
        
        if (GlobalFeatureManager.Instance.IsEnabled<PollsFeature>())
        {
            cmsProMenus.Add(new ApplicationMenuItem(
                    CmsKitProAdminMenus.Polls.PollMenu,
                    l["Polls"].Value,
                    "/Cms/Polls",
                    "fas fa-poll"
                ).RequirePermissions(CmsKitProAdminPermissions.Polls.Default)
            );
        }

        if (GlobalFeatureManager.Instance.IsEnabled<UrlShortingFeature>())
        {
            cmsProMenus.Add(new ApplicationMenuItem(
                    CmsKitProAdminMenus.UrlShorting.UrlShortingMenu,
                    l["UrlForwarding"].Value,
                    "/Cms/UrlShorting",
                     "fas fa-forward"
                ).RequirePermissions(CmsKitProAdminPermissions.UrlShorting.Default)
            );
        }
        
        if (GlobalFeatureManager.Instance.IsEnabled<FaqFeature>())
        {
            cmsProMenus.Add(new ApplicationMenuItem(
                    CmsKitProAdminMenus.Faqs.FaqMenu,
                    l["Faqs"].Value,
                    "/Cms/Faqs",
                     "fas fa-circle-question"
                ).RequirePermissions(CmsKitProAdminPermissions.Faqs.Default)
            );
        }
        
        if (cmsProMenus.Any())
        {
            var cmsMenu = context.Menu.FindMenuItem(CmsKitProAdminMenus.GroupName);

            if (cmsMenu == null)
            {
                cmsMenu = new ApplicationMenuItem(
                    CmsKitProAdminMenus.GroupName,
                    l["Cms"],
                    icon: "far fa-newspaper");

                context.Menu.AddItem(cmsMenu);
            }

            foreach (var cmsProMenu in cmsProMenus)
            {
                cmsMenu.AddItem(cmsProMenu);
            }

            // sort by alphabet
            var sortedCmsProMenus = cmsMenu.Items.OrderBy(x => x.DisplayName).ToList();
            cmsMenu.Items.Clear();
            for (var i = 0; i < sortedCmsProMenus.Count; i++)
            {
                var menuItem = sortedCmsProMenus[i];
                menuItem.Order = i;
                cmsMenu.Items.Add(menuItem);
            }
        }

        return Task.CompletedTask;
    }
}
