using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Requests;

[Serializable]
public class PaymentRequestStartDto : ExtensibleObject
{
    public Guid PaymentRequestId { get; set; }

    [Required]
    public string ReturnUrl { get; set; }

    public string CancelUrl { get; set; }
}
