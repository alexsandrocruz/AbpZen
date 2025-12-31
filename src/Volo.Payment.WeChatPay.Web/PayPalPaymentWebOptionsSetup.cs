using System;
using Microsoft.Extensions.Options;

namespace Volo.Payment.WeChatPay;

public class WeChatPayPaymentWebOptionsSetup : IConfigureOptions<PaymentWebOptions>
{
    protected WeChatPayWebOptions WeChatPayWebOptions { get; }

    public WeChatPayPaymentWebOptionsSetup(IOptions<WeChatPayWebOptions> alipayWebOptions)
    {
        WeChatPayWebOptions = alipayWebOptions.Value;
    }

    public void Configure(PaymentWebOptions options)
    {
        options.Gateways.Add(
            new PaymentGatewayWebConfiguration(
                WeChatPayConsts.GatewayName,
                WeChatPayConsts.PrePaymentUrl,
                isSubscriptionSupported: false,
                options.RootUrl.RemovePostFix("/") + WeChatPayConsts.PostPaymentUrl,
                WeChatPayWebOptions.Recommended,
                WeChatPayWebOptions.ExtraInfos
            )
        );
    }
}
