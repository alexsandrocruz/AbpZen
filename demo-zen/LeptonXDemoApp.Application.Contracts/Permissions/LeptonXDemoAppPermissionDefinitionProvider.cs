using LeptonXDemoApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LeptonXDemoApp.Permissions
{
    public class LeptonXDemoAppPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(LeptonXDemoAppPermissions.GroupName);

            myGroup.AddPermission(LeptonXDemoAppPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(LeptonXDemoAppPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            // <ZenCode-PermissionDefinition-Marker>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LeptonXDemoAppResource>(name);
        }
    }
}