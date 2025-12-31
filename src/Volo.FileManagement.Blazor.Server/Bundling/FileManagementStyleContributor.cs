using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.FileManagement.Blazor.Server.Bundling;

public class FileManagementStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/_content/Volo.FileManagement.Blazor/filemanagement/css/filemanagement.css");
    }
}
