using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Permissions;

public class CmsKitProPublicPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CmsKitProPublicPermissions.GroupName, L("Permission:Public"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsKitResource>(name);
    }
}
