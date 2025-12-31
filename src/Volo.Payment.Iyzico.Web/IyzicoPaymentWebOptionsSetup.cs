using Microsoft.Extensions.Options;
using System;

namespace Volo.Payment.Iyzico;

public class IyzicoPaymentWebOptionsSetup : IConfigureOptions<PaymentWebOptions>
{
    protected IyzicoWebOptions IyzicoWebOptions { get; }

    public IyzicoPaymentWebOptionsSetup(IOptions<IyzicoWebOptions> iyzicoOptions)
    {
        IyzicoWebOptions = iyzicoOptions.Value;
    }

    public void Configure(PaymentWebOptions options)
    {
        options.Gateways.Add(
            new PaymentGatewayWebConfiguration(
                IyzicoConsts.GatewayName,
                IyzicoConsts.PrePaymentUrl,
                isSubscriptionSupported: false,
                options.RootUrl.RemovePostFix("/") + IyzicoConsts.PostPaymentUrl,
                IyzicoWebOptions.Recommended,
                IyzicoWebOptions.ExtraInfos
            )
        );
    }
}
