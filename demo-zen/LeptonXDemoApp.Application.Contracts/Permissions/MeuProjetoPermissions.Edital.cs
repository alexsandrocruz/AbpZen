namespace LeptonXDemoApp.Permissions;

public static class MeuProjetoEditalPermissions
{
    public const string GroupName = "MeuProjeto";
    
    public const string Default = GroupName + ".Edital";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
