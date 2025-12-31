using System;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Requests;

namespace Volo.Payment.WeChatPay;

public class PurchaseParameterListGenerator : IPurchaseParameterListGenerator, ITransientDependency
{
    public virtual WeChatPayRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest)
    {
        return GetPaymentRequestExtraPropertyConfiguration(paymentRequest);
    }

    private WeChatPayRequestExtraParameterConfiguration GetPaymentRequestExtraPropertyConfiguration(PaymentRequest paymentRequest)
    {
        var configuration = new WeChatPayRequestExtraParameterConfiguration();

        if (!paymentRequest.ExtraProperties.ContainsKey(WeChatPayConsts.GatewayName))
        {
            return configuration;
        }
        
        var json = paymentRequest.ExtraProperties[WeChatPayConsts.GatewayName].ToString();
        var overrideConfiguration = Newtonsoft.Json.JsonConvert.DeserializeObject<WeChatPayRequestExtraParameterConfiguration>(json);

        if (!overrideConfiguration.AdditionalCallbackParameters.IsNullOrEmpty())
        {
            configuration.AdditionalCallbackParameters = overrideConfiguration.AdditionalCallbackParameters;
        }
        
        return configuration;
    }

}