using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;

public class PollsWidgetScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/client-proxies/cms-kit-pro-common-proxy.js");
        context.Files.Add("/client-proxies/cms-kit-pro-admin-proxy.js");
    }
}
