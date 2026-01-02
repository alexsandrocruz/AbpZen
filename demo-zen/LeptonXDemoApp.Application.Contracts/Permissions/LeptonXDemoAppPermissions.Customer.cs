namespace LeptonXDemoApp.Permissions;

public static class LeptonXDemoAppCustomerPermissions
{
    public const string GroupName = "LeptonXDemoApp";
    
    public const string Default = GroupName + ".Customer";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
