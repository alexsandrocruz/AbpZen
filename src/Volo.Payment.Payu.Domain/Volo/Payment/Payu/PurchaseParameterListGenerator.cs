using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Bcpg;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;
using Volo.Payment.Requests;

namespace Volo.Payment.Payu.Pages.Payment.Payu;

public class PurchaseParameterListGenerator : IPurchaseParameterListGenerator, ITransientDependency
{
    private readonly IClock _clock;
    private readonly PayuOptions _options;

    public PurchaseParameterListGenerator(
        IClock clock,
        IOptions<PayuOptions> options)
    {
        _clock = clock;
        _options = options.Value;
    }

    public virtual List<PurchaseParameter> Generate(PaymentRequest paymentRequest, string callbackUrl = null)
    {
        var paymentRequestConfiguration = GetPaymentRequestExtraPropertyConfiguration(paymentRequest);

        // Backward Support
        var currency = paymentRequest.Currency.IsNullOrEmpty() ? paymentRequestConfiguration.Currency : paymentRequest.Currency;

        var purchaseParameters = new List<PurchaseParameter>
            {
                new PurchaseParameter("LANGUAGE", _options.LanguageCode.ToUpper(), true),
                new PurchaseParameter("MERCHANT", _options.Merchant),
                new PurchaseParameter("ORDER_REF",  _clock.Now.ToString("yymmddhhMMss")),
                new PurchaseParameter("ORDER_DATE", _clock.Now.ToString("{yyyy-MM-dd HH:mm:ss}")),
            };

        var i = 0;
        foreach (var product in paymentRequest.Products)
        {
            purchaseParameters.Add(new PurchaseParameter("ORDER_PNAME[" + i + "]", product.Name));
            i++;
        }

        i = 0;
        foreach (var product in paymentRequest.Products)
        {
            purchaseParameters.Add(new PurchaseParameter("ORDER_PCODE[" + i + "]", product.Code));
            i++;
        }

        i = 0;
        foreach (var product in paymentRequest.Products)
        {
            purchaseParameters.Add(new PurchaseParameter("ORDER_PINFO[" + i + "]", ""));
            i++;
        }

        i = 0;
        foreach (var product in paymentRequest.Products)
        {
            purchaseParameters.Add(new PurchaseParameter("ORDER_PRICE[" + i + "]", product.UnitPrice.ToString(".00", CultureInfo.InvariantCulture)));
            i++;
        }

        i = 0;
        foreach (var product in paymentRequest.Products)
        {
            purchaseParameters.Add(new PurchaseParameter("ORDER_QTY[" + i + "]", product.Count.ToString()));
            i++;
        }

        i = 0;
        foreach (var product in paymentRequest.Products)
        {
            var productConfiguration = GetPaymentRequestProductExtraPropertyConfiguration(product);
            purchaseParameters.Add(new PurchaseParameter("ORDER_VAT[" + i + "]", productConfiguration.VatRate.ToString()));
        }

        purchaseParameters.AddRange(new List<PurchaseParameter>
            {
                new PurchaseParameter("ORDER_SHIPPING", paymentRequestConfiguration.Shipping.ToString()),
                new PurchaseParameter("PRICES_CURRENCY", currency.ToUpperInvariant()),
                new PurchaseParameter("DESTINATION_CITY", ""),
                new PurchaseParameter("DESTINATION_STATE", ""),
                new PurchaseParameter("DESTINATION_COUNTRY", ""),
                new PurchaseParameter("PAY_METHOD","CCVISAMC")
            });

        for (i = 0; i < paymentRequest.Products.Count; i++)
        {
            purchaseParameters.Add(new PurchaseParameter("ORDER_PRICE_TYPE[" + i + "]", paymentRequestConfiguration.PriceType));
        }

        purchaseParameters.AddRange(new List<PurchaseParameter>
            {
                new PurchaseParameter("INSTALLMENT_OPTIONS", paymentRequestConfiguration.Installment),
                new PurchaseParameter("BACK_REF", callbackUrl , true),
                new PurchaseParameter("TESTORDER",_options.TestOrder,true),
                new PurchaseParameter("DEBUG",_options.Debug, true)
            });

        var hashParameters = purchaseParameters.Where(p => !p.ExcludeFromHash).ToList();
        var hashParameter = new PurchaseParameter("ORDER_HASH", CalculateHashValue(hashParameters), true);
        purchaseParameters.Add(hashParameter);

        return purchaseParameters;
    }

    public virtual PayuPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest)
    {
        return GetPaymentRequestExtraPropertyConfiguration(paymentRequest);
    }

    protected virtual string CalculateHashValue(List<PurchaseParameter> parameters)
    {
        var str = new StringBuilder();
        foreach (var parameter in parameters)
        {
            str.Append(Encoding.UTF8.GetByteCount(parameter.Value));
            str.Append(parameter.Value);
        }

        var message = str.ToString();

        var hash = HmacMd5HashHelper.GetMd5Hash(message, _options.Signature);
        return hash;
    }

    protected virtual  PayuPaymentRequestExtraParameterConfiguration GetPaymentRequestExtraPropertyConfiguration(PaymentRequest paymentRequest)
    {
        var configuration = new PayuPaymentRequestExtraParameterConfiguration
        {
            Shipping = _options.Shipping,
            Installment = _options.Installment,
            PriceType = _options.PriceType,
            Currency = _options.CurrencyCode
        };

        // TODO: Remove currency fallback in next major version
        if (!paymentRequest.ExtraProperties.ContainsKey(PayuConsts.GatewayName))
        {
            return configuration;
        }

        var json = paymentRequest.ExtraProperties[PayuConsts.GatewayName].ToString();
        var overrideConfiguration = Newtonsoft.Json.JsonConvert.DeserializeObject<PayuPaymentRequestExtraParameterConfiguration>(json);

        if (!overrideConfiguration.PriceType.IsNullOrWhiteSpace())
        {
            configuration.PriceType = overrideConfiguration.PriceType;
        }

        if (!overrideConfiguration.Currency.IsNullOrWhiteSpace())
        {
            configuration.Currency = overrideConfiguration.Currency;
        }

        if (!overrideConfiguration.Installment.IsNullOrWhiteSpace())
        {
            configuration.Installment = overrideConfiguration.Installment;
        }

        if (overrideConfiguration.Shipping.HasValue)
        {
            configuration.Shipping = overrideConfiguration.Shipping;
        }

        if (!overrideConfiguration.AdditionalCallbackParameters.IsNullOrEmpty())
        {
            configuration.AdditionalCallbackParameters = overrideConfiguration.AdditionalCallbackParameters;
        }

        return configuration;
    }

    protected virtual  PayuPaymentRequestProductExtraParameterConfiguration GetPaymentRequestProductExtraPropertyConfiguration(PaymentRequestProduct product)
    {
        var configuration = new PayuPaymentRequestProductExtraParameterConfiguration
        {
            VatRate = _options.VatRate
        };

        if (!product.ExtraProperties.ContainsKey(PayuConsts.GatewayName))
        {
            return configuration;
        }

        var json = product.ExtraProperties[PayuConsts.GatewayName].ToString();
        var overrideConfiguration = Newtonsoft.Json.JsonConvert.DeserializeObject<PayuPaymentRequestProductExtraParameterConfiguration>(json);

        if (overrideConfiguration.VatRate.HasValue)
        {
            configuration.VatRate = overrideConfiguration.VatRate;
        }

        return configuration;
    }
}
