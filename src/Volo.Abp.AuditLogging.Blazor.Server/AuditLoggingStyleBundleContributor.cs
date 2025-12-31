using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AuditLogging.Blazor.Server;

public class AuditLoggingStyleBundleContributor: BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Volo.Abp.AuditLogging.Blazor/css/audit-logging.css");
    }
}