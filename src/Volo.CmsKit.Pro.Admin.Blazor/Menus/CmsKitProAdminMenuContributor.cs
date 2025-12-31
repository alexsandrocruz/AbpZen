using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Pro.Admin.Blazor.Menus;

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

    private async Task AddCmsMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CmsKitResource>();

        var cmsMenus = new List<ApplicationMenuItem>();

        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.Pages.PagesMenu,
                l["Pages"].Value,
                "/Cms/Pages",
                "fa fa-file-alt",
                order: 8
            )
            .RequireFeatures(CmsKitFeatures.PageEnable)
            .RequireGlobalFeatures(typeof(PagesFeature))
            .RequirePermissions(CmsKitAdminPermissions.Pages.Default));

        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.Blogs.BlogsMenu,
                l["Blogs"],
                "/Cms/Blogs",
                "fa fa-blog",
                order: 1
            )
            .RequireFeatures(CmsKitFeatures.BlogEnable)
            .RequireGlobalFeatures(typeof(BlogsFeature))
            .RequirePermissions(CmsKitAdminPermissions.Blogs.Default));

        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.BlogPosts.BlogPostsMenu,
                l["BlogPosts"],
                "/Cms/BlogPosts",
                "fa fa-file-signature",
                order: 2
            )
            .RequireFeatures(CmsKitFeatures.BlogEnable)
            .RequireGlobalFeatures(typeof(BlogsFeature))
            .RequirePermissions(CmsKitAdminPermissions.BlogPosts.Default));

        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.Tags.TagsMenu,
                l["Tags"].Value,
                "/Cms/Tags",
                "fa fa-tags",
                order: 10
            )
            .RequireFeatures(CmsKitFeatures.TagEnable)
            .RequireGlobalFeatures(typeof(TagsFeature))
            .RequirePermissions(CmsKitAdminPermissions.Tags.Default));

        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.Comments.CommentsMenu,
                l["Comments"].Value,
                "/Cms/Comments",
                "fa fa-comments",
                order: 3
            )
            .RequireFeatures(CmsKitFeatures.CommentEnable)
            .RequireGlobalFeatures(typeof(CommentsFeature))
            .RequirePermissions(CmsKitAdminPermissions.Comments.Default));

        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.Menus.MenusMenu,
                l["Menus"],
                "/Cms/Menus/Items",
                "fa fa-stream",
                order: 5
            )
            .RequireFeatures(CmsKitFeatures.MenuEnable)
            .RequireGlobalFeatures(typeof(MenuFeature))
            .RequirePermissions(CmsKitAdminPermissions.Menus.Default));

        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.GlobalResources.GlobalResourcesMenu,
                l["GlobalResources"],
                "/Cms/GlobalResources",
                "bi bi-code-slash",
                order: 4
            )
            .RequireFeatures(CmsKitFeatures.GlobalResourceEnable)
            .RequireGlobalFeatures(typeof(GlobalResourcesFeature))
            .RequirePermissions(CmsKitAdminPermissions.GlobalResources.Default));
        
        cmsMenus.Add(new ApplicationMenuItem( 
                CmsKitProAdminMenus.Newsletters.NewsletterMenu,
                l["NewsletterUsers"].Value,
                "/CmsKit/Newsletters",
                "fas fa-newspaper",
                order: 6
            )
            .RequireFeatures(CmsKitProFeatures.NewsletterEnable)
            .RequireGlobalFeatures(typeof(NewslettersFeature))
            .RequirePermissions(CmsKitProAdminPermissions.Newsletters.Default)
        );


        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.UrlShorting.UrlShortingMenu,
                l["UrlForwarding"].Value,
                "/Cms/UrlShorting",
                "fas fa-forward",
                order: 7
            )
            .RequireFeatures(CmsKitProFeatures.UrlShortingEnable)
            .RequireGlobalFeatures(typeof(UrlShortingFeature))
            .RequirePermissions(CmsKitProAdminPermissions.UrlShorting.Default)
        );


        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.Polls.PollMenu,
                l["Polls"].Value,
                "/Cms/Polls",
                "fas fa-poll",
                order: 9
            )
            .RequireFeatures(CmsKitProFeatures.PollEnable)
            .RequireGlobalFeatures(typeof(PollsFeature))
            .RequirePermissions(CmsKitProAdminPermissions.Polls.Default)
        );
        
        cmsMenus.Add(new ApplicationMenuItem(
                CmsKitProAdminMenus.PageFeedbacks.PageFeedbackMenu,
                l["PageFeedbacks"].Value,
                "/Cms/PageFeedbacks",
                "fas fa-comment",
                order: 10
            )
            .RequireFeatures(CmsKitProFeatures.PageFeedbackEnable)
            .RequireGlobalFeatures(typeof(PageFeedbackFeature))
            .RequirePermissions(CmsKitProAdminPermissions.PageFeedbacks.Default)
        );

        if (cmsMenus.Any())
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

            foreach (var menu in cmsMenus)
            {
                cmsMenu.AddItem(menu);
            }
        }
    }
}