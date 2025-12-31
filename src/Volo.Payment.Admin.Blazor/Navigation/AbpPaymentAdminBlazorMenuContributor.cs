using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Volo.Payment.Admin.Permissions;
using Volo.Payment.Localization;

namespace Volo.Payment.Admin.Blazor.Navigation;

public class AbpPaymentAdminBlazorMenuContributor : IMenuContributor
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

    private async Task AddPaymentMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<PaymentResource>();

        var paymentMenu = new ApplicationMenuItem(
            AbpPaymentAdminMenuNames.GroupName,
            l["Menu:PaymentManagement"],
            icon: "fa fa-money-check");

        context.Menu.GetAdministration().AddItem(paymentMenu);

        if (await context.IsGrantedAsync(PaymentAdminPermissions.Plans.Default))
        {
            paymentMenu.AddItem(new ApplicationMenuItem(
                AbpPaymentAdminMenuNames.Plans.PlansMenu,
                l["Plans"].Value,
                "/payment/plans",
                "fa fa-file-alt"));
        }

        if (await context.IsGrantedAsync(PaymentAdminPermissions.PaymentRequests.Default))
        {
            paymentMenu.AddItem(new ApplicationMenuItem(
                AbpPaymentAdminMenuNames.Requests.RequestsMenu,
                l["Menu:PaymentRequests"].Value,
                "/payment/requests",
                "fa fa-file-alt"));
        }
    }
}
