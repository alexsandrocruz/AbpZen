using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Contact;

public class ContactWidgetScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/client-proxies/cms-kit-pro-common-proxy.js");
        context.Files.Add("/Pages/Public/Shared/Components/Contact/Default.js");
    }
}
