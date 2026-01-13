namespace Sapienza.EventoZen.Permissions;

public static class LocationPermissions
{
    public const string GroupName = "EventoZen";
    
    public const string Default = GroupName + ".Location";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
