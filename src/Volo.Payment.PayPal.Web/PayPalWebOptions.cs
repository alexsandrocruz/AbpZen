using System.Collections.Generic;

namespace Volo.Payment.Paypal;

public class PayPalWebOptions
{
    public bool Recommended { get; set; }

    public List<string> ExtraInfos { get; set; }

    public PayPalWebOptions()
    {
        ExtraInfos = new();
    }
}
