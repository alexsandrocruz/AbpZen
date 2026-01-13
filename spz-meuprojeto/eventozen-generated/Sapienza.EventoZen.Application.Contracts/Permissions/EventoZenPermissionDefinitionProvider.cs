using Sapienza.EventoZen.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Sapienza.EventoZen.Permissions
{
    public class EventoZenPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(EventoZenPermissions.GroupName);

            myGroup.AddPermission(EventoZenPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(EventoZenPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(EventoZenPermissions.MyPermission1, L("Permission:MyPermission1"));
            // <<ZenCode-PermissionDefinition-Marker>>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<EventoZenResource>(name);
        }
    }
}