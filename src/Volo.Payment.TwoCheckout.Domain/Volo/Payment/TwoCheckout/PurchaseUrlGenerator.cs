using System;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Requests;

namespace Volo.Payment.TwoCheckout.Pages.Payment.TwoCheckout;

public class PurchaseUrlGenerator : IPurchaseUrlGenerator, ITransientDependency
{
    private readonly TwoCheckoutOptions _options;
    private readonly TwoCheckoutHashCalculator _twoCheckoutHashCalculator;

    public PurchaseUrlGenerator(
        IOptions<TwoCheckoutOptions> options,
        TwoCheckoutHashCalculator twoCheckoutHashCalculator)
    {
        _twoCheckoutHashCalculator = twoCheckoutHashCalculator;
        _options = options.Value;
    }

    public virtual TwoCheckoutPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(
        PaymentRequest paymentRequest, string returnUrl)
    {
        var configuration = new TwoCheckoutPaymentRequestExtraParameterConfiguration
        {
            Currency = _options.CurrencyCode,
            Language = _options.LanguageCode
        };

        if (!paymentRequest.ExtraProperties.ContainsKey(TwoCheckoutConsts.GatewayName))
        {
            return configuration;
        }

        var json = paymentRequest.ExtraProperties[TwoCheckoutConsts.GatewayName].ToString();
        var overrideConfiguration = Newtonsoft.Json.JsonConvert
            .DeserializeObject<TwoCheckoutPaymentRequestExtraParameterConfiguration>(json);

        // TODO: Remove currency fallback in next major version
        if (!overrideConfiguration.Currency.IsNullOrWhiteSpace())
        {
            configuration.Currency = overrideConfiguration.Currency;
        }

        if (!overrideConfiguration.Language.IsNullOrWhiteSpace())
        {
            configuration.Language = overrideConfiguration.Language;
        }

        if (!overrideConfiguration.AdditionalCallbackParameters.IsNullOrEmpty())
        {
            configuration.AdditionalCallbackParameters = overrideConfiguration.AdditionalCallbackParameters;
        }

        return configuration;
    }

    public virtual string GetUrl(PaymentRequest paymentRequest, string returnUrl)
    {
        var checkoutUrl = _options.CheckoutUrl.EnsureEndsWith('?');
        var backRefUrl = returnUrl;

        // TODO: Remove currency fallback in next major version.
        // Backward Support
        var currency = paymentRequest.Currency.IsNullOrEmpty() ? _options.CurrencyCode : paymentRequest.Currency;

        var hashQueryStringParameters = "PRODS=" + GetTwoCheckoutProductCodes(paymentRequest) + "&";
        hashQueryStringParameters += "QTY=" + GetTwoCheckoutProductCounts(paymentRequest) + "&";
        foreach (var product in paymentRequest.Products)
        {
            var productCode = product.ExtraProperties
                .GetPaymentRequestProductExtraPropertyConfiguration()
                .ProductCode;

            var price = $"{product.TotalPrice:0.00}";
            hashQueryStringParameters += "PRICES" + productCode + "[" + _options.CurrencyCode + "]=" + price + "&";
        }

        checkoutUrl += hashQueryStringParameters;
        checkoutUrl += "BACK_REF=" + WebUtility.UrlEncode(backRefUrl) + "&";

        if (!_options.CurrencyCode.IsNullOrEmpty())
        {
            checkoutUrl += "CURRENCY=" + currency + "&";
        }

        if (!_options.LanguageCode.IsNullOrEmpty())
        {
            checkoutUrl += "LANGUAGES=" + _options.LanguageCode + "&";
        }

        if (_options.TestOrder)
        {
            checkoutUrl += "DOTEST=1&";
        }

        var hash = _twoCheckoutHashCalculator.GetHmacSha256HashForQueryStringParameters(
            hashQueryStringParameters.EnsureEndsWith('&') + "BACK_REF=" + backRefUrl
        );

        checkoutUrl += "PHASH=sha256." + hash + "&";
        checkoutUrl += "CLEAN_CART=1&";
        checkoutUrl += "CARD=1&";
        checkoutUrl += "SECURE_CART=1&";
        checkoutUrl += "REF=" + paymentRequest.Id + "&";

        return checkoutUrl.TrimEnd('&');
    }

    private string GetTwoCheckoutProductCodes(PaymentRequest paymentRequest)
    {
        return string.Join(",",
            paymentRequest.Products.Select(
                product => product.ExtraProperties
                    .GetPaymentRequestProductExtraPropertyConfiguration()
                    .ProductCode
            )
        );
    }

    private string GetTwoCheckoutProductCounts(PaymentRequest paymentRequest)
    {
        return string.Join(",", paymentRequest.Products.Select(product => product.Count));
    }
}
