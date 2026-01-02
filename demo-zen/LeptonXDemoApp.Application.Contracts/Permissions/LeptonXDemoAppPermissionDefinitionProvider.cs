using LeptonXDemoApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LeptonXDemoApp.Permissions
{
    public class LeptonXDemoAppPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(LeptonXDemoAppPermissions.GroupName);

            myGroup.AddPermission(LeptonXDemoAppPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(LeptonXDemoAppPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            var editalGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.Edital.Default, L("Permission:Edital"));
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Create, L("Permission:Edital.Create"));
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Update, L("Permission:Edital.Update"));
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Delete, L("Permission:Edital.Delete"));

            var productGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.Product.Default, L("Permission:Product"));
            productGroup.AddChild(LeptonXDemoAppPermissions.Product.Create, L("Permission:Product.Create"));
            productGroup.AddChild(LeptonXDemoAppPermissions.Product.Update, L("Permission:Product.Update"));
            productGroup.AddChild(LeptonXDemoAppPermissions.Product.Delete, L("Permission:Product.Delete"));

            var categoryGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.Category.Default, L("Permission:Category"));
            categoryGroup.AddChild(LeptonXDemoAppPermissions.Category.Create, L("Permission:Category.Create"));
            categoryGroup.AddChild(LeptonXDemoAppPermissions.Category.Update, L("Permission:Category.Update"));
            categoryGroup.AddChild(LeptonXDemoAppPermissions.Category.Delete, L("Permission:Category.Delete"));
            // <ZenCode-PermissionDefinition-Marker>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LeptonXDemoAppResource>(name);
        }
    }
}