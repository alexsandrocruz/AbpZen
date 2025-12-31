using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Payment.Requests;

public class PaymentRequestProduct : Entity, IHasExtraProperties
{
    public Guid PaymentRequestId { get; private set; }

    [NotNull]
    public string Code { get; private set; }

    [NotNull]
    public string Name { get; private set; }

    public float UnitPrice { get; private set; }

    public int Count { get; private set; }

    public float TotalPrice { get; private set; }

    public PaymentType PaymentType { get; private set; }

    public Guid? PlanId { get; private set; }

    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    protected PaymentRequestProduct()
    {
        ExtraProperties = new ExtraPropertyDictionary();
    }

    /// <summary>
    /// Creates a PaymentRequestProduct.
    /// </summary>
    /// <param name="paymentRequestId"></param>
    /// <param name="code"></param>
    /// <param name="name"></param>
    /// <param name="paymentType"></param>
    /// <param name="unitPrice">Unit Price of Product. Required when PaymentType is <see cref="PaymentType.OneTime"/></param>
    /// <param name="count">Count of the Product.</param>
    /// <param name="planId">Required when PaymentType is <see cref="PaymentType.Subscription"/></param>
    /// <param name="totalPrice"></param>
    internal PaymentRequestProduct(
        Guid paymentRequestId,
        [NotNull] string code,
        [NotNull] string name,
        PaymentType paymentType = PaymentType.OneTime,
        float? unitPrice = null,
        int count = 1,
        Guid? planId = null,
        float? totalPrice = null)
    {
        PaymentRequestId = paymentRequestId;
        Code = Check.NotNullOrWhiteSpace(code, nameof(code));
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));

        if (paymentType == PaymentType.Subscription && planId == null)
        {
            throw new ArgumentNullException(nameof(planId), $"{nameof(planId)} is required when payment type is {PaymentType.Subscription}");
        }

        if (paymentType == PaymentType.OneTime && unitPrice == null)
        {
            throw new ArgumentNullException(nameof(unitPrice), $"{nameof(unitPrice)} is required when payment type is {PaymentType.OneTime}");
        }

        UnitPrice = unitPrice.Value;
        Count = count;
        TotalPrice = totalPrice ?? (UnitPrice * Count);
        PlanId = planId;
        PaymentType = paymentType;
        ExtraProperties = new ExtraPropertyDictionary();
    }

    public override object[] GetKeys()
    {
        return new object[] { PaymentRequestId, Code };
    }
}
