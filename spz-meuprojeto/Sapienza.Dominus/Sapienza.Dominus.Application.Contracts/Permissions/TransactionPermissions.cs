namespace Sapienza.Dominus.Permissions;

public static class TransactionPermissions
{
    public const string GroupName = "Dominus";
    
    public const string Default = GroupName + ".Transaction";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
