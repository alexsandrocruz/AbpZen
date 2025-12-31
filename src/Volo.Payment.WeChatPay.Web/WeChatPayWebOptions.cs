using System.Collections.Generic;

namespace Volo.Payment.WeChatPay;

public class WeChatPayWebOptions
{
    public string PrePaymentCheckoutButtonStyle { get; set; }

    public bool Recommended { get; set; }

    public List<string> ExtraInfos { get; set; }
    
    public string AesKey { get; set; }

    public WeChatPayWebOptions()
    {
        ExtraInfos = new();
    }
}