namespace Sapienza.Cursos.Permissions;

public static class LawyerPermissions
{
    public const string GroupName = "Sapienza.Cursos";
    
    public const string Default = GroupName + ".Lawyer";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
