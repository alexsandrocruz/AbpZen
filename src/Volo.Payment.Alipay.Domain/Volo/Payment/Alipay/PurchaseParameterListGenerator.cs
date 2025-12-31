using System;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Requests;

namespace Volo.Payment.Alipay;

public class PurchaseParameterListGenerator : IPurchaseParameterListGenerator, ITransientDependency
{
    public AlipayPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest)
    {
        return GetPaymentRequestExtraPropertyConfiguration(paymentRequest);
    }

    private AlipayPaymentRequestExtraParameterConfiguration GetPaymentRequestExtraPropertyConfiguration(PaymentRequest paymentRequest)
    {
        var configuration = new AlipayPaymentRequestExtraParameterConfiguration();

        if (!paymentRequest.ExtraProperties.ContainsKey(AlipayConsts.GatewayName))
        {
            return configuration;
        }
        
        var json = paymentRequest.ExtraProperties[AlipayConsts.GatewayName].ToString();
        var overrideConfiguration = Newtonsoft.Json.JsonConvert.DeserializeObject<AlipayPaymentRequestExtraParameterConfiguration>(json);

        if (!overrideConfiguration.AdditionalCallbackParameters.IsNullOrEmpty())
        {
            configuration.AdditionalCallbackParameters = overrideConfiguration.AdditionalCallbackParameters;
        }
        
        return configuration;
    }

}
