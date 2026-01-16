using Sapienza.Lexus.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Sapienza.Lexus.Permissions
{
    public class LexusPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(LexusPermissions.GroupName);

            myGroup.AddPermission(LexusPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(LexusPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(LexusPermissions.MyPermission1, L("Permission:MyPermission1"));
            // <<ZenCode-PermissionDefinition-Marker>>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LexusResource>(name);
        }
    }
}