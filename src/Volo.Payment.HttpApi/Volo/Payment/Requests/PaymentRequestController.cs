using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Volo.Payment.Requests;

[RemoteService(Name = AbpPaymentCommonRemoteServiceConsts.RemoteServiceName)]
[Area(AbpPaymentCommonRemoteServiceConsts.ModuleName)]
[Route("api/payment")]
public class PaymentRequestController : PaymentCommonController, IPaymentRequestAppService
{
    protected IPaymentRequestAppService PaymentRequestAppService { get; }

    public PaymentRequestController(IPaymentRequestAppService paymentRequestAppService)
    {
        PaymentRequestAppService = paymentRequestAppService;
    }

    [HttpPost]
    [Route("{paymentMethod}/complete")]
    public virtual Task<PaymentRequestWithDetailsDto> CompleteAsync(string paymentMethod, Dictionary<string, string> parameters)
    {
        return PaymentRequestAppService.CompleteAsync(paymentMethod, parameters);
    }

    [HttpPost]
    [Route("requests")]
    public virtual Task<PaymentRequestWithDetailsDto> CreateAsync(PaymentRequestCreateDto input)
    {
        return PaymentRequestAppService.CreateAsync(input);
    }

    [HttpGet]
    [Route("requests/{id}")]
    public virtual Task<PaymentRequestWithDetailsDto> GetAsync(Guid id)
    {
        return PaymentRequestAppService.GetAsync(id);
    }

    [HttpPost]
    [Route("{paymentMethod}/webhook")]
    public virtual async Task<bool> HandleWebhookAsync(string paymentMethod, string payload, [FromHeader]Dictionary<string,string> headers)
    {
        payload ??= await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        
        return await PaymentRequestAppService.HandleWebhookAsync(
            paymentMethod,
            payload,
            Request.Headers
                .ToDictionary(k => k.Key, v => v.Value.ToString()));
    }

    [HttpPost]
    [Route("{paymentMethod}/start")]
    public virtual Task<PaymentRequestStartResultDto> StartAsync(string paymentMethod, PaymentRequestStartDto input)
    {
        return PaymentRequestAppService.StartAsync(paymentMethod, input);
    }
}
