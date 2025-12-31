using System.Collections.Generic;

namespace Volo.Payment.Stripe;

public class StripeWebOptions
{
    public bool Recommended { get; set; }

    public List<string> ExtraInfos { get; set; }

    public StripeWebOptions()
    {
        ExtraInfos = new List<string>();
    }
}
