using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Requests;

[Serializable]
public class PaymentRequestCreateDto : ExtensibleObject
{
    public string Currency { get; set; }
    public List<PaymentRequestProductCreateDto> Products { get; set; }

    public PaymentRequestCreateDto()
    {
        Products = new List<PaymentRequestProductCreateDto>();
    }
}
