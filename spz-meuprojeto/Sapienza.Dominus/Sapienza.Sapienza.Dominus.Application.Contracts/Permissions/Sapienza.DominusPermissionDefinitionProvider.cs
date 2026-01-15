using Sapienza.Sapienza.Dominus.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Sapienza.Sapienza.Dominus.Permissions
{
    public class Sapienza.DominusPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(Sapienza.DominusPermissions.GroupName);

            myGroup.AddPermission(Sapienza.DominusPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(Sapienza.DominusPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(Sapienza.DominusPermissions.MyPermission1, L("Permission:MyPermission1"));
            // <<ZenCode-PermissionDefinition-Marker>>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<Sapienza.DominusResource>(name);
        }
    }
}