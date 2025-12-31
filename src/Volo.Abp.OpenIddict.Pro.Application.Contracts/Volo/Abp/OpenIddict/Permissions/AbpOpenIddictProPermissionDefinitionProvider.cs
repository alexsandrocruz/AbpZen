using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.OpenIddict.Permissions;

public class AbpOpenIddictProPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var openIddictGroup = context.AddGroup(AbpOpenIddictProPermissions.GroupName, L("Permission:OpenIddictPro"));

        var application = openIddictGroup.AddPermission(AbpOpenIddictProPermissions.Application.Default, L("Permission:Application"), MultiTenancySides.Host);
        application.AddChild(AbpOpenIddictProPermissions.Application.Update, L("Permission:Edit"), MultiTenancySides.Host);
        application.AddChild(AbpOpenIddictProPermissions.Application.Delete, L("Permission:Delete"), MultiTenancySides.Host);
        application.AddChild(AbpOpenIddictProPermissions.Application.Create, L("Permission:Create"), MultiTenancySides.Host);
        application.AddChild(AbpOpenIddictProPermissions.Application.ManagePermissions, L("Permission:ManagePermissions"), MultiTenancySides.Host);
        application.AddChild(AbpOpenIddictProPermissions.Application.ViewChangeHistory, L("Permission:ViewChangeHistory"), MultiTenancySides.Host);

        var scope = openIddictGroup.AddPermission(AbpOpenIddictProPermissions.Scope.Default, L("Permission:Scope"), MultiTenancySides.Host);
        scope.AddChild(AbpOpenIddictProPermissions.Scope.Update, L("Permission:Edit"), MultiTenancySides.Host);
        scope.AddChild(AbpOpenIddictProPermissions.Scope.Delete, L("Permission:Delete"), MultiTenancySides.Host);
        scope.AddChild(AbpOpenIddictProPermissions.Scope.Create, L("Permission:Create"), MultiTenancySides.Host);
        scope.AddChild(AbpOpenIddictProPermissions.Scope.ViewChangeHistory, L("Permission:ViewChangeHistory"), MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpOpenIddictResource>(name);
    }
}
