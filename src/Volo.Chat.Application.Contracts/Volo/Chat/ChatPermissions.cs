using Volo.Abp.Reflection;

namespace Volo.Chat.Authorization;

public static class ChatPermissions
{
    public const string GroupName = "Chat";

    public const string Messaging = GroupName + ".Messaging";
    public const string Searching = GroupName + ".Searching";
    
    public const string SettingManagement = GroupName + ".SettingManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ChatPermissions));
    }
}
