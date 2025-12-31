using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Gateways;
using Volo.Payment.Payu.Pages.Payment.Payu;
using Volo.Payment.Requests;

namespace Volo.Payment.Payu;

public class PayuPaymentGateway : IPaymentGateway, ITransientDependency
{
    protected IPaymentRequestRepository PaymentRequestRepository { get; }

    protected IPurchaseParameterListGenerator PurchaseParameterListGenerator { get; }

    protected PayuOptions PayuOptions { get; }

    public PayuPaymentGateway(
        IPaymentRequestRepository paymentRequestRepository,
        IPurchaseParameterListGenerator purchaseParameterListGenerator,
        IOptions<PayuOptions> payuOptions)
    {
        PaymentRequestRepository = paymentRequestRepository;
        PurchaseParameterListGenerator = purchaseParameterListGenerator;
        PayuOptions = payuOptions.Value;
    }

    public virtual Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input)
    {
        var purchaseParameters = PurchaseParameterListGenerator.Generate(paymentRequest);

        var resultDto = new PaymentRequestStartResult
        {
            CheckoutLink = PayuOptions.CheckoutLink,
        };

        foreach (var parameter in purchaseParameters)
        {
            resultDto.ExtraProperties.Add(parameter.Key, parameter.Value);
        }

        return Task.FromResult(resultDto);
    }

    public virtual async Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters)
    {
        var paymentRequestId = Guid.Parse(parameters[PayuConsts.ParameterNames.PaymentRequestId]);

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
        var controlString = properties["ctrl"]?.ToString();

        if (controlString.IsNullOrWhiteSpace())
        {
            throw new Exception("Empty control string.");
        }

        var url = properties["url"]?.ToString();

        if (url.IsNullOrWhiteSpace())
        {
            throw new Exception("Empty url parameter.");
        }

        var urlWithoutControlString = url.Replace("&ctrl=" + controlString, "").Replace("?ctrl=" + controlString, "");
        var hashString = urlWithoutControlString.Length + urlWithoutControlString;
        var hash = HmacMd5HashHelper.GetMd5Hash(hashString, PayuOptions.Signature);
        return hash == controlString.ToLowerInvariant();
    }
}
