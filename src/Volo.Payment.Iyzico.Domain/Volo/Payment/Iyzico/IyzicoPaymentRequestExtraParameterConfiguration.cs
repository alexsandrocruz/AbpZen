using JetBrains.Annotations;

namespace Volo.Payment.Iyzico;

public class IyzicoPaymentRequestExtraParameterConfiguration
{
    /// <summary>
    /// Shipping fee
    /// </summary>
    public string Locale { get; set; }

    /// <summary>
    /// Currency code ex: USD, EUR
    /// </summary>
    [CanBeNull]
    public string Currency { get; set; }

    [CanBeNull]
    public string AdditionalCallbackParameters { get; set; }
}
