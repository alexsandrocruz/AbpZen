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

        public static class Edital
        {
            public const string Default = GroupName + ".Edital";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
      // <ZenCode-Permissions-Marker>
    }
}
