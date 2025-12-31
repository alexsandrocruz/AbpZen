using System.Collections.Generic;

namespace Volo.Payment.Iyzico;

public class IyzicoWebOptions
{
    public string PrePaymentCheckoutButtonStyle { get; set; }

    public bool Recommended { get; set; }

    public List<string> ExtraInfos { get; set; }

    public IyzicoWebOptions()
    {
        ExtraInfos = new();
    }
}
