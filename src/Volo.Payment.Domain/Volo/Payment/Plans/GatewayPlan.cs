using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Payment.Plans;

public class GatewayPlan : Entity, IHasExtraProperties
{
    public Guid PlanId { get; protected set; }

    [NotNull]
    public string Gateway { get; protected set; }

    [NotNull]
    public string ExternalId { get; protected set; }

    public ExtraPropertyDictionary ExtraProperties { get; }

    protected GatewayPlan()
    {
    }

    public GatewayPlan(
        Guid planId,
        [NotNull] string gateway,
        [NotNull] string externalId,
        [CanBeNull] ExtraPropertyDictionary extraProperties = null)
    {
        PlanId = planId;
        Gateway = Check.NotNullOrEmpty(gateway, nameof(gateway));
        SetExternalId(externalId);
        ExtraProperties = extraProperties ?? new ExtraPropertyDictionary();
    }

    public void SetExternalId([NotNull] string externalId)
    {
        ExternalId = Check.NotNullOrEmpty(externalId, nameof(externalId));
    }

    public override object[] GetKeys()
    {
        return new object[] { PlanId, Gateway };
    }
}
