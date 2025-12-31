namespace Volo.Payment.Admin.Permissions;

public static class PaymentAdminPermissions
{
    public const string GroupName = "Payment";

    public static class Plans
    {
        public const string Default = GroupName + ".Plans";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";

        public static class GatewayPlans
        {
            public const string Default = Plans.Default + ".GatewayPlans";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }

    public static class PaymentRequests
    {
        public const string Default = GroupName + ".PaymentRequests";
    }
}
