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
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Create, L("Permission:Create"));
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Update, L("Permission:Update"));
            editalGroup.AddChild(LeptonXDemoAppPermissions.Edital.Delete, L("Permission:Delete"));

            var productGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.Product.Default, L("Permission:Product"));
            productGroup.AddChild(LeptonXDemoAppPermissions.Product.Create, L("Permission:Create"));
            productGroup.AddChild(LeptonXDemoAppPermissions.Product.Update, L("Permission:Update"));
            productGroup.AddChild(LeptonXDemoAppPermissions.Product.Delete, L("Permission:Delete"));

            var categoryGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.Category.Default, L("Permission:Category"));
            categoryGroup.AddChild(LeptonXDemoAppPermissions.Category.Create, L("Permission:Create"));
            categoryGroup.AddChild(LeptonXDemoAppPermissions.Category.Update, L("Permission:Update"));
            categoryGroup.AddChild(LeptonXDemoAppPermissions.Category.Delete, L("Permission:Delete"));

            var orderGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.Order.Default, L("Permission:Order"));
            orderGroup.AddChild(LeptonXDemoAppPermissions.Order.Create, L("Permission:Create"));
            orderGroup.AddChild(LeptonXDemoAppPermissions.Order.Update, L("Permission:Create"));
            orderGroup.AddChild(LeptonXDemoAppPermissions.Order.Delete, L("Permission:Delete"));

            var customerGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.Customer.Default, L("Permission:Customer"));
            customerGroup.AddChild(LeptonXDemoAppPermissions.Customer.Create, L("Permission:Create"));
            customerGroup.AddChild(LeptonXDemoAppPermissions.Customer.Update, L("Permission:Update"));
            customerGroup.AddChild(LeptonXDemoAppPermissions.Customer.Delete, L("Permission:Delete"));

            var orderItemGroup = myGroup.AddPermission(LeptonXDemoAppPermissions.OrderItem.Default, L("Permission:OrderItem"));
            orderItemGroup.AddChild(LeptonXDemoAppPermissions.OrderItem.Create, L("Permission:Create"));
            orderItemGroup.AddChild(LeptonXDemoAppPermissions.OrderItem.Update, L("Permission:Update"));
            orderItemGroup.AddChild(LeptonXDemoAppPermissions.OrderItem.Delete, L("Permission:Delete"));

            var leadPermission = myGroup.AddPermission(LeptonXDemoAppPermissions.Lead.Default, L("Permission:Lead"));
            leadPermission.AddChild(LeptonXDemoAppPermissions.Lead.Create, L("Permission:Create"));
            leadPermission.AddChild(LeptonXDemoAppPermissions.Lead.Update, L("Permission:Update"));
            leadPermission.AddChild(LeptonXDemoAppPermissions.Lead.Delete, L("Permission:Delete"));

            var leadContactPermission = myGroup.AddPermission(LeptonXDemoAppPermissions.LeadContact.Default, L("Permission:LeadContact"));
            leadContactPermission.AddChild(LeptonXDemoAppPermissions.LeadContact.Create, L("Permission:Create"));
            leadContactPermission.AddChild(LeptonXDemoAppPermissions.LeadContact.Update, L("Permission:Update"));
            leadContactPermission.AddChild(LeptonXDemoAppPermissions.LeadContact.Delete, L("Permission:Delete"));

            var messageTemplatePermission = myGroup.AddPermission(LeptonXDemoAppPermissions.MessageTemplate.Default, L("Permission:MessageTemplate"));
            messageTemplatePermission.AddChild(LeptonXDemoAppPermissions.MessageTemplate.Create, L("Permission:Create"));
            messageTemplatePermission.AddChild(LeptonXDemoAppPermissions.MessageTemplate.Update, L("Permission:Update"));
            messageTemplatePermission.AddChild(LeptonXDemoAppPermissions.MessageTemplate.Delete, L("Permission:Delete"));

            var leadMessagePermission = myGroup.AddPermission(LeptonXDemoAppPermissions.LeadMessage.Default, L("Permission:LeadMessage"));
            leadMessagePermission.AddChild(LeptonXDemoAppPermissions.LeadMessage.Create, L("Permission:Create"));
            leadMessagePermission.AddChild(LeptonXDemoAppPermissions.LeadMessage.Update, L("Permission:Update"));
            leadMessagePermission.AddChild(LeptonXDemoAppPermissions.LeadMessage.Delete, L("Permission:Delete"));
      // <ZenCode-PermissionDefinition-Marker>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LeptonXDemoAppResource>(name);
        }
    }
}