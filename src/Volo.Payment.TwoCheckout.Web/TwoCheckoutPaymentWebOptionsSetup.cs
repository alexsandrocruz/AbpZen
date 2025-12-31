using System;
using Microsoft.Extensions.Options;

namespace Volo.Payment.TwoCheckout;

public class TwoCheckoutPaymentWebOptionsSetup : IConfigureOptions<PaymentWebOptions>
{
    protected TwoCheckoutWebOptions TwoCheckoutWebOptions { get; }

    public TwoCheckoutPaymentWebOptionsSetup(IOptions<TwoCheckoutWebOptions> twoCheckoutOptions)
    {
        TwoCheckoutWebOptions = twoCheckoutOptions.Value;
    }

    public void Configure(PaymentWebOptions options)
    {
        options.Gateways.Add(
            new PaymentGatewayWebConfiguration(
                TwoCheckoutConsts.GatewayName,
                TwoCheckoutConsts.PrePaymentUrl,
                isSubscriptionSupported: false,
                options.RootUrl.RemovePostFix("/") + TwoCheckoutConsts.PostPaymentUrl,
                TwoCheckoutWebOptions.Recommended,
                TwoCheckoutWebOptions.ExtraInfos
            )
        );
    }
}
