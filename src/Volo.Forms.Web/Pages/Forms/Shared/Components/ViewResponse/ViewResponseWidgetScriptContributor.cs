using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Forms.Web.Pages.Forms.Shared.Components.ViewResponse;

public class ViewResponseWidgetScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/client-proxies/form-proxy.js");
        context.Files.Add("/Pages/Forms/Shared/Components/ViewResponse/Default.js");
    }
}
