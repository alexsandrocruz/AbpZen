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

            // Generated Permissions
            var clientGroup = myGroup.AddPermission(ClientPermissions.Default, L("Permission:Client"));
            clientGroup.AddChild(ClientPermissions.Create, L("Permission:Create"));
            clientGroup.AddChild(ClientPermissions.Update, L("Permission:Update"));
            clientGroup.AddChild(ClientPermissions.Delete, L("Permission:Delete"));

            var artistGroup = myGroup.AddPermission(ArtistPermissions.Default, L("Permission:Artist"));
            artistGroup.AddChild(ArtistPermissions.Create, L("Permission:Create"));
            artistGroup.AddChild(ArtistPermissions.Update, L("Permission:Update"));
            artistGroup.AddChild(ArtistPermissions.Delete, L("Permission:Delete"));

            var artistSpecialtyGroup = myGroup.AddPermission(ArtistSpecialtyPermissions.Default, L("Permission:ArtistSpecialty"));
            artistSpecialtyGroup.AddChild(ArtistSpecialtyPermissions.Create, L("Permission:Create"));
            artistSpecialtyGroup.AddChild(ArtistSpecialtyPermissions.Update, L("Permission:Update"));
            artistSpecialtyGroup.AddChild(ArtistSpecialtyPermissions.Delete, L("Permission:Delete"));

            var availabilityGroup = myGroup.AddPermission(AvailabilityPermissions.Default, L("Permission:Availability"));
            availabilityGroup.AddChild(AvailabilityPermissions.Create, L("Permission:Create"));
            availabilityGroup.AddChild(AvailabilityPermissions.Update, L("Permission:Update"));
            availabilityGroup.AddChild(AvailabilityPermissions.Delete, L("Permission:Delete"));

            var eventGroup = myGroup.AddPermission(EventPermissions.Default, L("Permission:Event"));
            eventGroup.AddChild(EventPermissions.Create, L("Permission:Create"));
            eventGroup.AddChild(EventPermissions.Update, L("Permission:Update"));
            eventGroup.AddChild(EventPermissions.Delete, L("Permission:Delete"));

            var eventCommissionGroup = myGroup.AddPermission(EventCommissionPermissions.Default, L("Permission:EventCommission"));
            eventCommissionGroup.AddChild(EventCommissionPermissions.Create, L("Permission:Create"));
            eventCommissionGroup.AddChild(EventCommissionPermissions.Update, L("Permission:Update"));
            eventCommissionGroup.AddChild(EventCommissionPermissions.Delete, L("Permission:Delete"));

            var locationGroup = myGroup.AddPermission(LocationPermissions.Default, L("Permission:Location"));
            locationGroup.AddChild(LocationPermissions.Create, L("Permission:Create"));
            locationGroup.AddChild(LocationPermissions.Update, L("Permission:Update"));
            locationGroup.AddChild(LocationPermissions.Delete, L("Permission:Delete"));

            // <<ZenCode-PermissionDefinition-Marker>>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<EventoZenResource>(name);
        }
    }
}