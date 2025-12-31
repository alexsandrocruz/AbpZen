using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Payment.Gateways;

namespace Volo.Payment;

public class PaymentGatewayConfiguration
{
    [NotNull]
    public string Name { get; }

    [NotNull]
    public ILocalizableString DisplayName {
        get => _displayName;
        set => _displayName = Check.NotNull(value, nameof(value));
    }
    private ILocalizableString _displayName;

    /// <summary>
    /// Must be a type that implements <see cref="IPaymentGateway"/>
    /// </summary>
    [NotNull]
    public Type PaymentGatewayType { get; }

    /// <summary>
    /// Default value: 1000.
    /// </summary>
    public int Order { get; set; } = 1000;

    public bool IsSubscriptionSupported { get; }

    public PaymentGatewayConfiguration(
        [NotNull] string name,
        [NotNull] ILocalizableString displayName,
        bool isSubscriptionSupported,
        [NotNull] Type paymentGatewayType)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        DisplayName = displayName;
        IsSubscriptionSupported = isSubscriptionSupported;
        PaymentGatewayType = paymentGatewayType;
    }
}
