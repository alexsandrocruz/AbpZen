namespace Sapienza.FabioRibeiro.Permissions
{
    public static class FabioRibeiroPermissions
    {
        public const string GroupName = "Sapienza.FabioRibeiro";

        public static class Dashboard
        {
            public const string DashboardGroup = GroupName + ".Dashboard";
            public const string Host = DashboardGroup + ".Host";
            public const string Tenant = DashboardGroup + ".Tenant";
        }

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";
        // <ZenCode-Permissions-Marker>
    }
}
