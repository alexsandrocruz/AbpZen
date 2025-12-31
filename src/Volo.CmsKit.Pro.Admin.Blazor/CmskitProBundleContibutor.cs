using Volo.Abp.Bundling;

namespace Volo.CmsKit.Pro.Admin.Blazor;

public class CmsKitProBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {
        context.Add("_content/Volo.CmsKit.Pro.Admin.Blazor/libs/easymde/easymde.min.js");
        context.Add("_content/Volo.CmsKit.Pro.Admin.Blazor/libs/easymde/highlight.min.js");
    }

    public void AddStyles(BundleContext context)
    {
        context.Add("_content/Volo.CmsKit.Pro.Admin.Blazor/libs/easymde/easymde.min.css");
        context.Add("_content/Blazorise.TreeView/blazorise.treeview.css");
    }
}
