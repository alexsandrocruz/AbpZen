using System.Collections.Generic;

namespace Volo.Payment.Stripe;

public class StripeOptions
{
    public string PublishableKey { get; set; }

    public string SecretKey { get; set; }

    public string Currency { get; set; }

    public string Locale { get; set; }

    public string WebhookSecret { get; set; }

    public List<string> PaymentMethodTypes { get; set; }

    public StripeOptions()
    {
        Currency = "USD";
        Locale = "auto";

        PaymentMethodTypes = new List<string>
            {
                "card"
            };
    }
}
