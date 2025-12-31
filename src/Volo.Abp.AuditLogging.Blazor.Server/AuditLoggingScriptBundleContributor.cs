using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AuditLogging.Blazor.Server;

public class AuditLoggingScriptBundleContributor: BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Volo.Abp.AuditLogging.Blazor/libs/chart/chart.min.js");
    }
}