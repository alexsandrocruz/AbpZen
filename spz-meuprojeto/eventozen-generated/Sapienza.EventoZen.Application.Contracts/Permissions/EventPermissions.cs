namespace Sapienza.EventoZen.Permissions;

public static class EventPermissions
{
    public const string GroupName = "EventoZen";
    
    public const string Default = GroupName + ".Event";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
