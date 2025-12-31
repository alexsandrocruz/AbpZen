using System;
using Volo.Abp.Data;

namespace Volo.Payment.TwoCheckout;

public static class PaymentRequestProductExtensions
{
    public static TwoCheckoutPaymentRequestProductExtraParameterConfiguration
        GetPaymentRequestProductExtraPropertyConfiguration(this ExtraPropertyDictionary extraProperties)
    {
        if (!extraProperties.ContainsKey(TwoCheckoutConsts.GatewayName))
        {
            throw new ArgumentException(
                message: "Two checkout extra parameters are not configured for this product !"
            );
        }

        var json = extraProperties[TwoCheckoutConsts.GatewayName].ToString();
        var extraParameters = Newtonsoft.Json.JsonConvert
            .DeserializeObject<TwoCheckoutPaymentRequestProductExtraParameterConfiguration>(json);

        if (extraParameters.ProductCode.IsNullOrEmpty())
        {
            throw new ArgumentException(message: "Two checkout product code is not configured for this product !");
        }

        return new TwoCheckoutPaymentRequestProductExtraParameterConfiguration
        {
            ProductCode = extraParameters.ProductCode
        };
    }
}
