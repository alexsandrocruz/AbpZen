using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Payment.Plans;
using Volo.Payment.Requests;

namespace Volo.Payment.DemoApp.Pages;

public class SubscriptionModel : PageModel
{
    private IPaymentRequestAppService PaymentRequestAppService { get; }

    public SubscriptionModel(
        IPaymentRequestAppService paymentRequestAppService)
    {
        PaymentRequestAppService = paymentRequestAppService;
    }

    public virtual async Task<IActionResult> OnPost()
    {
        var paymentRequest = await PaymentRequestAppService.CreateAsync(
             new PaymentRequestCreateDto
             {
                 Products =
                 {
                        new PaymentRequestProductCreateDto
                        {
                            PaymentType = PaymentType.Subscription,
                            Name = DemoAppData.Plan_2_Name,
                            Code = "EP",
                            Count = 1,
                            PlanId = DemoAppData.Plan_2_Id,
                        }
                 }
             });

        return LocalRedirectPreserveMethod("/Payment/GatewaySelection?paymentRequestId=" + paymentRequest.Id);
    }
}
