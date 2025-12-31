using System;
using Microsoft.Extensions.Options;
using Volo.Payment.Paypal;

namespace Volo.Payment.PayPal;

public class PayPalPaymentWebOptionsSetup : IConfigureOptions<PaymentWebOptions>
{
    protected PayPalWebOptions PayPalWebOptions { get; }

    public PayPalPaymentWebOptionsSetup(IOptions<PayPalWebOptions> payPalOptions)
    {
        PayPalWebOptions = payPalOptions.Value;
    }

    public void Configure(PaymentWebOptions options)
    {
        options.Gateways.Add(
            new PaymentGatewayWebConfiguration(
                PayPalConsts.GatewayName,
                PayPalConsts.PrePaymentUrl,
                isSubscriptionSupported: false,
                options.RootUrl.RemovePostFix("/") + PayPalConsts.PostPaymentUrl,
                PayPalWebOptions.Recommended,
                PayPalWebOptions.ExtraInfos
            )
        );
    }
}
