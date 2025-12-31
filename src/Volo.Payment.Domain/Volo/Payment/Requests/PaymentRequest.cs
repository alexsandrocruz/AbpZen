using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Payment.Requests;

public class PaymentRequest : CreationAuditedAggregateRoot<Guid>
{
    public virtual ICollection<PaymentRequestProduct> Products { get; protected set; }

    public PaymentRequestState State { get; private set; }

    /// <summary>
    /// Three Letter ISO3 Currency Code. Ex.: USD, EUR, GBP, etc.
    /// </summary>
    [CanBeNull]
    public string Currency { get; set; }

    [CanBeNull]
    public string Gateway { get; set; }

    [CanBeNull]
    public string FailReason { get; private set; }

    public DateTime? EmailSendDate { get; set; }

    public string ExternalSubscriptionId { get; protected set; }

    protected PaymentRequest()
    {
    }

    public PaymentRequest(Guid id, [CanBeNull] string currency = null)
    {
        Id = id;
        Products = new List<PaymentRequestProduct>();
        Currency = currency;
    }

    public virtual void SetExternalSubscriptionId([NotNull] string externalSubscriptionId)
    {
        if (!ExternalSubscriptionId.IsNullOrEmpty() && ExternalSubscriptionId != externalSubscriptionId)
        {
            throw new BusinessException(PaymentDomainErrorCodes.Requests.CantUpdateExternalSubscriptionId);
        }

        ExternalSubscriptionId = Check.NotNullOrEmpty(externalSubscriptionId, nameof(externalSubscriptionId));
    }

    public PaymentRequestProduct AddProduct(
        [NotNull] string code,
        [NotNull] string name,
        PaymentType paymentType = PaymentType.OneTime,
        float unitPrice = 0,
        int count = 1,
        Guid? planId = null,
        float? totalPrice = null,
        Dictionary<string, IPaymentRequestProductExtraParameterConfiguration> extraProperties = null)
    {
        if (paymentType == PaymentType.OneTime && Currency.IsNullOrEmpty())
        {
            throw new BusinessException(AbpPaymentErrorCodes.CurrencyMustBeSet, "Currency of this PaymentRequest is empty. It must be set to continue with OneTime Payment.");
        }

        var product = new PaymentRequestProduct(
            Id,
            code,
            name,
            paymentType,
            unitPrice,
            count: count,
            planId: planId,
            totalPrice: totalPrice
        );

        if (extraProperties != null)
        {
            foreach (var extraProperty in extraProperties)
            {
                product.SetProperty(extraProperty.Key, extraProperty.Value);
            }
        }

        Products.Add(product);

        return product;
    }

    public virtual void Waiting()
    {
        State = PaymentRequestState.Waiting;
    }

    public virtual void Complete()
    {
        if (State == PaymentRequestState.Completed)
        {
            return;
        }

        if (State != PaymentRequestState.Waiting && State != PaymentRequestState.Failed)
        {
            throw new ApplicationException(
                $"Can not complete a payment in '{State}' state!"
            );
        }

        State = PaymentRequestState.Completed;
    }

    public virtual void Failed([CanBeNull] string reason = null)
    {
        if (State != PaymentRequestState.Waiting && State != PaymentRequestState.Failed)
        {
            throw new ApplicationException(
                $"Can not fail a payment in '{State}' state!"
            );
        }

        State = PaymentRequestState.Failed;
        FailReason = reason;
    }

    public virtual void Refunded()
    {
        if (State != PaymentRequestState.Completed)
        {
            throw new ApplicationException(
                $"Can not refund a payment in '{State}' state!"
            );
        }

        State = PaymentRequestState.Refunded;
    }

    public virtual void SetState(PaymentRequestState state)
    {
        switch (state)
        {
            case PaymentRequestState.Waiting:
                Waiting();
                break;
            case PaymentRequestState.Completed:
                Complete();
                break;
            case PaymentRequestState.Failed:
                Failed();
                break;
            case PaymentRequestState.Refunded:
                Refunded();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
