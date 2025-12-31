using JetBrains.Annotations;

namespace Volo.Payment.WeChatPay;

public class WeChatPayRequestExtraParameterConfiguration
{
    [CanBeNull]
    public string AdditionalCallbackParameters { get; set; }
}