using System;

namespace Volo.Payment.TwoCheckout;

public class TwoCheckoutOptions
{
    public string Signature { get; set; }

    public string CheckoutUrl { get; set; }

    public string LanguageCode { get; set; }

    [Obsolete("Use Currency property of PaymentRequest")]
    public string CurrencyCode { get; set; }

    public bool TestOrder { get; set; }
}
