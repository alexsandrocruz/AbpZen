namespace LeptonXDemoApp.Permissions
{
    public static class LeptonXDemoAppPermissions
    {
        public const string GroupName = "LeptonXDemoApp";

        public static class Dashboard
        {
            public const string DashboardGroup = GroupName + ".Dashboard";
            public const string Host = DashboardGroup + ".Host";
            public const string Tenant = DashboardGroup + ".Tenant";
        }

        // <ZenCode-Permissions-Marker>
    }
}
