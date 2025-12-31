using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Gateways;

public class PaymentRequestStartResult : ExtensibleObject
{
    public string CheckoutLink { get; set; }
}
