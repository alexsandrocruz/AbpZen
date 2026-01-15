namespace Sapienza.Dominus.Permissions;

public static class BlogCategoryPermissions
{
    public const string GroupName = "Dominus";
    
    public const string Default = GroupName + ".BlogCategory";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
