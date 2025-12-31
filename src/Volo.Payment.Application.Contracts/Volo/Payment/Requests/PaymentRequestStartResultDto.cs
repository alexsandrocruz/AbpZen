using System;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Requests;

[Serializable]
public class PaymentRequestStartResultDto : ExtensibleObject
{
    public string CheckoutLink { get; set; }
}
