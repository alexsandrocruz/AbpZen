using JetBrains.Annotations;

namespace Volo.Payment.Alipay;

public class AlipayPaymentRequestExtraParameterConfiguration
{
    [CanBeNull]
    public string AdditionalCallbackParameters { get; set; }
}
