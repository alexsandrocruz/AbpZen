using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Gateways;
using Volo.Payment.Requests;
using Volo.Payment.TwoCheckout.Pages.Payment.TwoCheckout;

namespace Volo.Payment.TwoCheckout;

public class TwoCheckoutPaymentGateway : IPaymentGateway, ITransientDependency
{
    protected IPaymentRequestRepository PaymentRequestRepository { get; }

    protected IPurchaseUrlGenerator PurchaseUrlGenerator { get; }

    protected TwoCheckoutOptions TwoCheckoutOptions { get; }
    
    protected const string ReturnUrlPropertyName = "ReturnUrl";

    public TwoCheckoutPaymentGateway(
        IPaymentRequestRepository paymentRequestRepository,
        IPurchaseUrlGenerator purchaseUrlGenerator,
        IOptions<TwoCheckoutOptions> twoCheckoutOptions)
    {
        PaymentRequestRepository = paymentRequestRepository;
        PurchaseUrlGenerator = purchaseUrlGenerator;
        TwoCheckoutOptions = twoCheckoutOptions.Value;
    }

    public virtual async Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input)
    {
        paymentRequest.SetProperty(ReturnUrlPropertyName, input.ReturnUrl);
        
        await PaymentRequestRepository.UpdateAsync(paymentRequest);
        
        return new PaymentRequestStartResult
        {
            CheckoutLink = PurchaseUrlGenerator.GetUrl(paymentRequest, input.ReturnUrl)
        };
    }

    public virtual async Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters)
    {
        var paymentRequestId = Guid.Parse(parameters[TwoCheckoutConsts.ParameterNames.PaymentRequestId]);
        var paymentRequest = await PaymentRequestRepository.GetAsync(paymentRequestId);

        if (IsValid(paymentRequest, parameters))
        {
            paymentRequest.Complete();
        }
        else
        {
            paymentRequest.Failed();
        }

        return await PaymentRequestRepository.UpdateAsync(paymentRequest);
    }

    public virtual Task HandleWebhookAsync(string payload, Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public virtual bool IsValid(PaymentRequest paymentRequest, Dictionary<string, string> properties)
    {
        var sha256 = properties["hmac-sha256"];
        if (sha256.IsNullOrWhiteSpace())
        {
            throw new Exception("Empty encryption code.");
        }

        var url = paymentRequest.GetProperty<string>(ReturnUrlPropertyName);
        if (url.IsNullOrWhiteSpace())
        {
            throw new Exception("Empty url parameter.");
        }

        var urlWithoutSha256 = url.Replace("&hmac-sha256=" + sha256, "").Replace("?hmac-sha256=" + sha256, "");
        var hashString = urlWithoutSha256.Length + urlWithoutSha256;
        var hash = HmacSha256HashHelper.GetHmacSha256Hash(hashString, TwoCheckoutOptions.Signature);

        return hash == sha256.ToLowerInvariant();
    }
}
