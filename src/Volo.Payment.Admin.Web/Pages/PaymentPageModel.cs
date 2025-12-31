using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Payment.Admin.Web.Pages;

public class PaymentPageModel : AbpPageModel
{
    public PaymentPageModel()
    {
        ObjectMapperContext = typeof(AbpPaymentAdminWebModule);
    }
}
