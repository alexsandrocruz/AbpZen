using Volo.Abp.Reflection;

namespace Volo.Forms.Permissions;

public class FormsPermissions
{
    public const string GroupName = "Forms";

    public static class Forms
    {
        public const string Default = GroupName + ".Form";
        public const string Delete = Default + ".Delete";
    }

    public static class Response
    {
        public const string Default = GroupName + ".Response";
        public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FormsPermissions));
    }
}
