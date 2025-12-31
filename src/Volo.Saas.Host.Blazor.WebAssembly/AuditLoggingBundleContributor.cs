using Volo.Abp.Bundling;

namespace Volo.Saas.Host.Blazor.WebAssembly;

public class SaasBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {
        context.Add("_content/Volo.Saas.Host.Blazor/libs/chart/chart.min.js");
    }

    public void AddStyles(BundleContext context)
    {
        
    }
}