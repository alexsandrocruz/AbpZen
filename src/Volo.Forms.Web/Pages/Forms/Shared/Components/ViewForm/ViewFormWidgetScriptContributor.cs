using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Forms.Web.Pages.Forms.Shared.Components.ViewForm;

public class ViewFormWidgetScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/client-proxies/form-proxy.js");
        context.Files.Add("/Pages/Forms/Shared/Components/ViewForm/Vue-email-property.js");
        context.Files.Add("/Pages/Forms/Shared/Components/ViewForm/Vue-answer.js");
        context.Files.Add("/Pages/Forms/Shared/Components/ViewForm/Default.js");
    }
}
