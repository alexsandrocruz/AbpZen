using Sapienza.FabioRibeiro.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Sapienza.FabioRibeiro.Permissions
{
    public class FabioRibeiroPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(FabioRibeiroPermissions.GroupName);

            myGroup.AddPermission(FabioRibeiroPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(FabioRibeiroPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(FabioRibeiroPermissions.MyPermission1, L("Permission:MyPermission1"));
            // <<ZenCode-PermissionDefinition-Marker>>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FabioRibeiroResource>(name);
        }
    }
}