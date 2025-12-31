using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;
using Volo.Payment.Localization;

namespace Volo.Payment.Admin.Web.Pages;

public class PaymentAdminPageBase : Microsoft.AspNetCore.Mvc.RazorPages.Page
{
    [RazorInject] public IStringLocalizer<PaymentResource> L { get; set; }

    [RazorInject] public IPageLayout PageLayout { get; set; }

    public override Task ExecuteAsync()
    {
        return Task.CompletedTask; // Will be overridden by razor pages. (.cshtml)
    }
}
