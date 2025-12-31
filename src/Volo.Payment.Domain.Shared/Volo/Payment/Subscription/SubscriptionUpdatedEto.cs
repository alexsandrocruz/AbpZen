using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Payment.Requests;

namespace Volo.Payment.Subscription;

[Serializable]
[EventName("Volo.Payment.RecurringPaymentUpdated")]
public class SubscriptionUpdatedEto : EtoBase, IHasExtraProperties
{
    public Guid PaymentRequestId { get; set; }

    public PaymentRequestState State { get; set; }

    public string Currency { get; set; }

    public string Gateway { get; set; }

    public string ExternalSubscriptionId { get; set; }

    public DateTime PeriodEndDate { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }
}
