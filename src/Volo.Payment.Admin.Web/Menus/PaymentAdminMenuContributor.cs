using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Payment.Admin.Permissions;
using Volo.Payment.Localization;

namespace Volo.Payment.Admin.Web.Menus;

public class PaymentAdminMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        await AddPaymentMenuAsync(context);
    }

    private Task AddPaymentMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<PaymentResource>();

        var paymentMenu = new ApplicationMenuItem(
            PaymentAdminMenus.GroupName,
            l["Menu:PaymentManagement"],
            icon: "fa fa-money-check");

        context.Menu.GetAdministration().AddItem(paymentMenu);

        paymentMenu.AddItem(new ApplicationMenuItem(
            PaymentAdminMenus.Plans.PlansMenu,
            l["Menu:Plans"].Value,
            "/Payment/Plans",
            "fa fa-file-alt"))
        .RequirePermissions(PaymentAdminPermissions.Plans.Default);

        paymentMenu.AddItem(new ApplicationMenuItem(
            PaymentAdminMenus.PaymentRequests.PaymentRequestsMenu,
            l["Menu:PaymentRequests"].Value,
            "/Payment/Requests",
            "fa fa-exchange"))
        .RequirePermissions(PaymentAdminPermissions.PaymentRequests.Default);

        return Task.CompletedTask;
    }
}
