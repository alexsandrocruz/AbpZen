using Volo.FileManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace Volo.FileManagement.Authorization;

public class FileManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var fileManagementGroup = context.AddGroup(FileManagementPermissions.GroupName, L("Permission:FileManagement"));

        var directoryPermission = fileManagementGroup.AddPermission(FileManagementPermissions.DirectoryDescriptor.Default, L("Permission:FileManagement:Directory"))
            .RequireFeatures(FileManagementFeatures.Enable);
        directoryPermission.AddChild(FileManagementPermissions.DirectoryDescriptor.Create, L("Permission:FileManagement:Directory:Create"))
            .RequireFeatures(FileManagementFeatures.Enable);
        directoryPermission.AddChild(FileManagementPermissions.DirectoryDescriptor.Update, L("Permission:FileManagement:Directory:Update"))
            .RequireFeatures(FileManagementFeatures.Enable);
        directoryPermission.AddChild(FileManagementPermissions.DirectoryDescriptor.Delete, L("Permission:FileManagement:Directory:Delete"))
            .RequireFeatures(FileManagementFeatures.Enable);

        var filePermission = fileManagementGroup.AddPermission(FileManagementPermissions.FileDescriptor.Default, L("Permission:FileManagement:File"))
            .RequireFeatures(FileManagementFeatures.Enable);
        filePermission.AddChild(FileManagementPermissions.FileDescriptor.Create, L("Permission:FileManagement:File:Create"))
            .RequireFeatures(FileManagementFeatures.Enable);
        filePermission.AddChild(FileManagementPermissions.FileDescriptor.Update, L("Permission:FileManagement:File:Update"))
            .RequireFeatures(FileManagementFeatures.Enable);
        filePermission.AddChild(FileManagementPermissions.FileDescriptor.Delete, L("Permission:FileManagement:File:Delete"))
            .RequireFeatures(FileManagementFeatures.Enable);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FileManagementResource>(name);
    }
}
