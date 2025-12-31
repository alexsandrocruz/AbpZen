using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Payment.Requests;

namespace Volo.Payment.Requests;

[Serializable]
public class PaymentRequestWithDetailsDto : ExtensibleEntityDto<Guid>, IHasCreationTime
{
    public virtual List<PaymentRequestProductDto> Products { get; set; }

    /// <summary>
    /// Currency code ex: USD, EUR
    /// </summary>
    public string Currency { get; set; }

    public PaymentRequestState State { get; set; }

    public string FailReason { get; set; }

    public DateTime? EmailSendDate { get; set; }

    public string Gateway { get; set; }

    public string ExternalSubscriptionId { get; set; }
    public float TotalPrice { get; set; }

    public DateTime CreationTime { get; set; }
}
