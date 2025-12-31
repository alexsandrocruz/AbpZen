using System;
using Microsoft.Extensions.Options;

namespace Volo.Payment.Payu;

public class PayuPaymentWebOptionsSetup : IConfigureOptions<PaymentWebOptions>
{
    protected PayuWebOptions PayuWebOptions { get; }

    public PayuPaymentWebOptionsSetup(IOptions<PayuWebOptions> payuOptions)
    {
        PayuWebOptions = payuOptions.Value;
    }

    public void Configure(PaymentWebOptions options)
    {
        options.Gateways.Add(
            new PaymentGatewayWebConfiguration(
                PayuConsts.GatewayName,
                PayuConsts.PrePaymentUrl,
                isSubscriptionSupported: false,
                options.RootUrl.RemovePostFix("/") + PayuConsts.PostPaymentUrl,
                PayuWebOptions.Recommended,
                PayuWebOptions.ExtraInfos
            )
        );
    }
}
