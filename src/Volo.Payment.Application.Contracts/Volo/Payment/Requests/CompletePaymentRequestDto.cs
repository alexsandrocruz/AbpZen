using System;
using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Requests;

public class CompletePaymentRequestDto : ExtensibleObject
{
    public Guid Id { get; set; }

    public string GateWay { get; set; }

    public bool IsSubscription { get; set; }

    public CompletePaymentRequestSubscriptionDto SubscriptionInfo { get; set; }
}
