using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.CmsKit.Pro.Admin.Web.Bundles;

public class SlugifyScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/libs/slugify/slugify.js");
    }
}
