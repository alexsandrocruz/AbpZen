namespace Sapienza.Cursos.Permissions;

public static class TurmaPermissions
{
    public const string GroupName = "Cursos";
    
    public const string Default = GroupName + ".Turma";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
