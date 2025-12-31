using System;
using Microsoft.Extensions.Options;

namespace Volo.Payment.Stripe;

public class StripePaymentWebOptionsSetup : IConfigureOptions<PaymentWebOptions>
{
    protected StripeWebOptions StripeWebOptions { get; }

    public StripePaymentWebOptionsSetup(IOptions<StripeWebOptions> stripeOptions)
    {
        StripeWebOptions = stripeOptions.Value;
    }

    public void Configure(PaymentWebOptions options)
    {
        options.Gateways.Add(
            new PaymentGatewayWebConfiguration(
                StripeConsts.GatewayName,
                StripeConsts.PrePaymentUrl,
                isSubscriptionSupported: true,
                options.RootUrl.RemovePostFix("/") + StripeConsts.PostPaymentUrl,
                StripeWebOptions.Recommended,
                StripeWebOptions.ExtraInfos
            )
        );
    }
}
