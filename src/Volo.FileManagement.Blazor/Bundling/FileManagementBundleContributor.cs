using Volo.Abp.Bundling;

namespace Volo.FileManagement.Blazor.Bundling;

public class FileManagementBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {

    }

    public void AddStyles(BundleContext context)
    {
        context.Add("_content/Volo.FileManagement.Blazor/filemanagement/css/filemanagement.css");
    }
}
