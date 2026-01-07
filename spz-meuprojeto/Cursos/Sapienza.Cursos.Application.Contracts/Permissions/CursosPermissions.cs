namespace Sapienza.Cursos.Permissions
{
    public static class CursosPermissions
    {
        public const string GroupName = "Sapienza.Cursos";

        public static class Dashboard
        {
            public const string DashboardGroup = GroupName + ".Dashboard";
            public const string Host = DashboardGroup + ".Host";
            public const string Tenant = DashboardGroup + ".Tenant";
        }

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";
                public static class Turma
        {
            public const string Default = GroupName + ".Turma";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
      // <ZenCode-Permissions-Marker>
    }
}
