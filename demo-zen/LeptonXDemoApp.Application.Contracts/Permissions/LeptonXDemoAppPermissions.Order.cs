namespace LeptonXDemoApp.Permissions;

public static class LeptonXDemoAppOrderPermissions
{
    public const string GroupName = "LeptonXDemoApp";
    
    public const string Default = GroupName + ".Order";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
