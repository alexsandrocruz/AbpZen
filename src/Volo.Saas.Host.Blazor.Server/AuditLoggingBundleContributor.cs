using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Saas.Host.Blazor.Server;

public class SaasBundleContributor: BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Volo.Saas.Host.Blazor/libs/chart/chart.min.js");
    }
}