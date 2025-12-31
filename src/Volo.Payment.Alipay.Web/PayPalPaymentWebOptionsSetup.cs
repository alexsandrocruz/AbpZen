using System;
using Microsoft.Extensions.Options;

namespace Volo.Payment.Alipay;

public class AlipayPaymentWebOptionsSetup : IConfigureOptions<PaymentWebOptions>
{
    protected AlipayWebOptions AlipayWebOptions { get; }

    public AlipayPaymentWebOptionsSetup(IOptions<AlipayWebOptions> alipayWebOptions)
    {
        AlipayWebOptions = alipayWebOptions.Value;
    }

    public void Configure(PaymentWebOptions options)
    {
        options.Gateways.Add(
            new PaymentGatewayWebConfiguration(
                AlipayConsts.GatewayName,
                AlipayConsts.PrePaymentUrl,
                isSubscriptionSupported: false,
                options.RootUrl.RemovePostFix("/") + AlipayConsts.PostPaymentUrl,
                AlipayWebOptions.Recommended,
                AlipayWebOptions.ExtraInfos
            )
        );
    }
}
