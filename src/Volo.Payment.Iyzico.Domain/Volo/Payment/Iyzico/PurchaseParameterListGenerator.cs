using System;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Requests;

namespace Volo.Payment.Iyzico;

public class PurchaseParameterListGenerator : IPurchaseParameterListGenerator, ITransientDependency
{
    private readonly IyzicoOptions _options;

    public PurchaseParameterListGenerator(
        IOptions<IyzicoOptions> options)
    {
        _options = options.Value;
    }

    public virtual IyzicoPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest)
    {
        return GetPaymentRequestExtraPropertyConfiguration(paymentRequest);
    }

    private IyzicoPaymentRequestExtraParameterConfiguration GetPaymentRequestExtraPropertyConfiguration(PaymentRequest paymentRequest)
    {
        var configuration = new IyzicoPaymentRequestExtraParameterConfiguration
        {
            Currency = _options.Currency,
            Locale = _options.Locale,
        };

        if (!paymentRequest.ExtraProperties.ContainsKey(IyzicoConsts.GatewayName))
        {
            return configuration;
        }

        var json = paymentRequest.ExtraProperties[IyzicoConsts.GatewayName].ToString();
        var overrideConfiguration = Newtonsoft.Json.JsonConvert.DeserializeObject<IyzicoPaymentRequestExtraParameterConfiguration>(json);

        if (!overrideConfiguration.Currency.IsNullOrWhiteSpace())
        {
            configuration.Currency = overrideConfiguration.Currency;
        }

        if (!overrideConfiguration.AdditionalCallbackParameters.IsNullOrEmpty())
        {
            configuration.AdditionalCallbackParameters = overrideConfiguration.AdditionalCallbackParameters;
        }

        return configuration;
    }
}
