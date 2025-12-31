using System.Collections.Generic;

namespace Volo.Payment.Alipay;

public class AlipayWebOptions
{
    public string PrePaymentCheckoutButtonStyle { get; set; }

    public bool Recommended { get; set; }

    public List<string> ExtraInfos { get; set; }

    public AlipayWebOptions()
    {
        ExtraInfos = new();
    }
}