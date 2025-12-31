using System.Collections.Generic;

namespace Volo.Payment.TwoCheckout;

public class TwoCheckoutWebOptions
{
    public bool Recommended { get; set; }

    public List<string> ExtraInfos { get; set; }

    public TwoCheckoutWebOptions()
    {
        ExtraInfos = new List<string>();
    }
}
