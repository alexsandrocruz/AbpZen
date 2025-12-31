using System.Collections.Generic;

namespace Volo.Payment.Payu;

public class PayuWebOptions
{
    public bool Recommended { get; set; }

    public List<string> ExtraInfos { get; set; }

    public string PrePaymentCheckoutButtonStyle { get; set; }

    public PayuWebOptions()
    {
        ExtraInfos = new List<string>();
    }
}
