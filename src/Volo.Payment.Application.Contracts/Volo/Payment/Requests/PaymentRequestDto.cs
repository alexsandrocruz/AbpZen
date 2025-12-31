using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Requests;

namespace Volo.Payment.Requests;

[Serializable]
public class PaymentRequestDto : ExtensibleEntityDto<Guid>
{
    /// <summary>
    /// Currency code ex: USD, EUR
    /// </summary>
    public string Currency { get; set; }

    public PaymentRequestState State { get; set; }

    public string FailReason { get; set; }

    public DateTime? EmailSendDate { get; set; }

    public string Gateway { get; set; }

    public string ExternalSubscriptionId { get; set; }
}
