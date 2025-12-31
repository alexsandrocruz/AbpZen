using System;
using System.Collections.Generic;

namespace Volo.Payment.PayPal;

public class PayPalOptions
{
    public string ClientId { get; set; }

    public string Secret { get; set; }

    [Obsolete("Use Currency property of PaymentRequest")]
    public string CurrencyCode { get; set; }

    public string Locale { get; set; }

    /// <summary>
    /// "Sandbox" or "Live". Default value is "Sandbox"
    /// </summary>
    public string Environment { get; set; }

    public PayPalOptions()
    {
        Environment = PayPalConsts.Environment.Sandbox;
    }
}
