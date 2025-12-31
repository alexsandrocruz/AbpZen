using Volo.Abp.Bundling;

namespace Volo.Abp.AuditLogging.Blazor;

public class AuditLoggingBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {
        context.Add("_content/Volo.Abp.AuditLogging.Blazor/libs/chart/chart.min.js");
    }

    public void AddStyles(BundleContext context)
    {
        context.Add("_content/Volo.Abp.AuditLogging.Blazor/css/audit-logging.css");
    }
}
