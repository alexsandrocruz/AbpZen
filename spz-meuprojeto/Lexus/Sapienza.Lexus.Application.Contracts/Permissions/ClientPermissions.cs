namespace Sapienza.Lexus.Permissions;

public static class ClientPermissions
{
    public const string GroupName = "Lexus";
    
    public const string Default = GroupName + ".Client";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
