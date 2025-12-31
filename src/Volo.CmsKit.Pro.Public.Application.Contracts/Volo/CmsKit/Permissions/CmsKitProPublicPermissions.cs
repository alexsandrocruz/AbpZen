using Volo.Abp.Reflection;

namespace Volo.CmsKit.Permissions;

public class CmsKitProPublicPermissions
{
    public const string GroupName = "Public";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CmsKitProPublicPermissions));
    }
}
