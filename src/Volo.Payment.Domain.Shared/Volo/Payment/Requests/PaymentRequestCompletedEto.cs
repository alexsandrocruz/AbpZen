using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.Payment.Requests;

[Serializable]
[EventName("Volo.Payment.PaymentRequestCompleted")]
public class PaymentRequestCompletedEto : EtoBase, IHasExtraProperties
{
    public Guid Id { get; }

    public string Gateway { get; set; }

    public string Currency { get; set; }

    public List<PaymentRequestProductCompletedEto> Products { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; }

    private PaymentRequestCompletedEto()
    {
        //Default constructor is needed for deserialization.
    }

    public PaymentRequestCompletedEto(
        Guid id,
        string gateway,
        string currency,
        List<PaymentRequestProductCompletedEto> products,
        ExtraPropertyDictionary extraProperties = null)
    {
        Id = id;
        Gateway = gateway;
        Currency = currency;
        Products = products;
        ExtraProperties = extraProperties ?? new ExtraPropertyDictionary();
    }
}

[Serializable]
public class PaymentRequestProductCompletedEto : EtoBase
{
    public string Code { get; set; }

    public string Name { get; set; }

    public int Count { get; set; }
}
