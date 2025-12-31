using System;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Requests;

namespace Volo.Payment.PayPal;

public class PurchaseParameterListGenerator : IPurchaseParameterListGenerator, ITransientDependency
{
    private readonly PayPalOptions _options;

    public PurchaseParameterListGenerator(
        IOptions<PayPalOptions> options)
    {
        _options = options.Value;
    }

    public virtual PayPalPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(
        PaymentRequest paymentRequest)
    {
        return GetPaymentRequestExtraPropertyConfiguration(paymentRequest);
    }

    private PayPalPaymentRequestExtraParameterConfiguration GetPaymentRequestExtraPropertyConfiguration(
        PaymentRequest paymentRequest)
    {
        var configuration = new PayPalPaymentRequestExtraParameterConfiguration
        {
            CurrencyCode = _options.CurrencyCode, // TODO: Remove currency fallback in next major version. Use paymentRequest.Currency as default here.
            Locale = _options.Locale
        };

        if (!paymentRequest.ExtraProperties.ContainsKey(PayPalConsts.GatewayName))
        {
            return configuration;
        }

        var json = paymentRequest.ExtraProperties[PayPalConsts.GatewayName].ToString();
        var overrideConfiguration = Newtonsoft.Json.JsonConvert
            .DeserializeObject<PayPalPaymentRequestExtraParameterConfiguration>(json);

        if (!overrideConfiguration.CurrencyCode.IsNullOrWhiteSpace())
        {
            configuration.CurrencyCode = overrideConfiguration.CurrencyCode;
        }

        if (!overrideConfiguration.Locale.IsNullOrWhiteSpace())
        {
            configuration.Locale = overrideConfiguration.Locale;
        }

        if (!overrideConfiguration.AdditionalCallbackParameters.IsNullOrEmpty())
        {
            configuration.AdditionalCallbackParameters = overrideConfiguration.AdditionalCallbackParameters;
        }

        return configuration;
    }
}
