using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Forms.Localization;

namespace Volo.Forms.Permissions;

public class FormsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var formManagementGroup = context.AddGroup(FormsPermissions.GroupName, L("Permission:Forms"));

        var formPermissions = formManagementGroup.AddPermission(FormsPermissions.Forms.Default, L("Permission:Forms.Management"))
            .RequireFeatures(FormsFeatures.Enable);
        formPermissions.AddChild(FormsPermissions.Forms.Delete, L("Permission:Forms:Form.Delete"))
            .RequireFeatures(FormsFeatures.Enable);

        var responsePermissions = formManagementGroup.AddPermission(FormsPermissions.Response.Default, L("Permission:Forms.Response.Management"))
            .RequireFeatures(FormsFeatures.Enable);
        responsePermissions.AddChild(FormsPermissions.Response.Delete, L("Permission:Forms:Response.Delete"))
            .RequireFeatures(FormsFeatures.Enable);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FormsResource>(name);
    }
}
