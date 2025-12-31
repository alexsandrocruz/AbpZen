using System;

namespace Volo.Payment.Payu;

public class PayuOptions
{
    public string CheckoutLink { get; set; } = "https://secure.payu.ro/order/lu.php";

    public string Merchant { get; set; }

    public string Signature { get; set; }

    public string LanguageCode { get; set; }

    [Obsolete("Use Currency property of PaymentRequest")]
    public string CurrencyCode { get; set; }

    public int VatRate { get; set; }

    public string PriceType { get; set; }

    public int Shipping { get; set; }

    public string Installment { get; set; }

    public string TestOrder { get; set; }

    public string Debug { get; set; }
}
