using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Payment.Admin.Requests;
using Volo.Payment.Requests;

namespace Volo.Payment.Admin.Web.Pages.Payment.Requests.Products;

public class IndexModel : PaymentPageModel
{
    protected IPaymentRequestAdminAppService PaymentRequestAppService { get; }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public PaymentRequestWithDetailsDto PaymentRequest { get; set; }

    public IndexModel(IPaymentRequestAdminAppService paymentRequestAppService)
    {
        PaymentRequestAppService = paymentRequestAppService;
    }

    public virtual async Task OnGetAsync()
    {
        PaymentRequest = await PaymentRequestAppService.GetAsync(Id);
    }
}
