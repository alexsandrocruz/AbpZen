using System;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Gateways;

public class PaymentRequestStartInput : ExtensibleObject
{
    public Guid PaymentRequestId { get; set; }

    public string ReturnUrl { get; set; }

    public string CancelUrl { get; set; }
}
