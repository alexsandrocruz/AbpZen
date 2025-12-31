using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.FileManagement.Authorization;
using Volo.FileManagement.Localization;

namespace Volo.FileManagement.Blazor.Navigation;

public class FileManagementMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var l = context.GetLocalizer<FileManagementResource>();

        var fileManagementMenuItem = new ApplicationMenuItem(FileManagementMenuNames.GroupName, l["Menu:FileManagement"], "~/file-management", icon: "fa fa-folder-open")
            .RequireFeatures(FileManagementFeatures.Enable)
            .RequirePermissions(FileManagementPermissions.DirectoryDescriptor.Default);

        context.Menu.AddItem(fileManagementMenuItem);

        return Task.CompletedTask;
    }
}
