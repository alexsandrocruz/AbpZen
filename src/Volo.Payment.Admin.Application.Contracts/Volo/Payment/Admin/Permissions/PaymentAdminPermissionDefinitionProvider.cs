using System;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Payment.Localization;

namespace Volo.Payment.Admin.Permissions;

public class PaymentAdminPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var paymentGroup = context.GetGroupOrNull(PaymentAdminPermissions.GroupName) ?? context.AddGroup(PaymentAdminPermissions.GroupName, L("Permission:Payment"));

        var planGroup = paymentGroup.AddPermission(PaymentAdminPermissions.Plans.Default, L("Permission:PaymentPlanManagement"), MultiTenancySides.Host);

        planGroup.AddChild(PaymentAdminPermissions.Plans.Create, L("Permission:PaymentPlanManagement.Create"), MultiTenancySides.Host);
        planGroup.AddChild(PaymentAdminPermissions.Plans.Update, L("Permission:PaymentPlanManagement.Update"), MultiTenancySides.Host);
        planGroup.AddChild(PaymentAdminPermissions.Plans.Delete, L("Permission:PaymentPlanManagement.Delete"), MultiTenancySides.Host);

        var gatewayPlanGroup = paymentGroup.AddPermission(
            PaymentAdminPermissions.Plans.GatewayPlans.Default,
            L("Permission:PaymentGatewayPlanManagement"),
            MultiTenancySides.Host);

        gatewayPlanGroup.AddChild(PaymentAdminPermissions.Plans.GatewayPlans.Create, L("Permission:PaymentGatewayPlanManagement.Create"), MultiTenancySides.Host);
        gatewayPlanGroup.AddChild(PaymentAdminPermissions.Plans.GatewayPlans.Update, L("Permission:PaymentGatewayPlanManagement.Update"), MultiTenancySides.Host);
        gatewayPlanGroup.AddChild(PaymentAdminPermissions.Plans.GatewayPlans.Delete, L("Permission:PaymentGatewayPlanManagement.Delete"), MultiTenancySides.Host);

        paymentGroup.AddPermission(PaymentAdminPermissions.PaymentRequests.Default, L("Permission:PaymentRequests"), MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PaymentResource>(name);
    }
}
