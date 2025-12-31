using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.CmsKit.Pro.Admin.Blazor.Server;

public class CmsKitProStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Volo.CmsKit.Pro.Admin.Blazor/libs/easymde/easymde.min.css");
        context.Files.AddIfNotContains("/_content/Blazorise.TreeView/blazorise.treeview.css");
    }
}