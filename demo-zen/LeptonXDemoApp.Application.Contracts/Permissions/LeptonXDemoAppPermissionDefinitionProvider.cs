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

            var editalGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.Edital.Default, L("Permission:Edital"));
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Create, L("Permission:Create"));
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Update, L("Permission:Update"));
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Delete, L("Permission:Delete"));
      // <ZenCode-PermissionDefinition-Marker>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LeptonXDemoAppResource>(name);
        }
    }
}