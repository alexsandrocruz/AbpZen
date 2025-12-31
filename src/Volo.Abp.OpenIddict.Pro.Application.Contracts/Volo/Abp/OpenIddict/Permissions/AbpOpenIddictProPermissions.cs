using Volo.Abp.Reflection;

namespace Volo.Abp.OpenIddict.Permissions;

public class AbpOpenIddictProPermissions
{
    public const string GroupName = "OpenIddictPro";

    public static class Application
    {
        public const string Default = GroupName + ".Application";
        public const string Delete = Default + ".Delete";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string ManagePermissions = Default + ".ManagePermissions";
        public const string ViewChangeHistory = "AuditLogging.ViewChangeHistory:Volo.Abp.OpenIddict.Pro.Applications.Application";
    }
    
    public static class Scope
    {
        public const string Default = GroupName + ".Scope";
        public const string Delete = Default + ".Delete";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string ViewChangeHistory = "AuditLogging.ViewChangeHistory:Volo.Abp.OpenIddict.Pro.Scopes.Scope";
    }
    
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AbpOpenIddictProPermissions));
    }
}
