namespace LeptonXDemoApp.Permissions;

public static class LeptonXDemoAppOrderItemPermissions
{
    public const string GroupName = "LeptonXDemoApp";
    
    public const string Default = GroupName + ".OrderItem";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
