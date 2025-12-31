using System;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Requests;

public class CompletePaymentRequestSubscriptionDto
{
    public string ExternalSubscriptionId { get; set; }

    public DateTime PeriodEndDate { get; set; }
}
