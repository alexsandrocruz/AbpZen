using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Payment.Plans;

public class Plan : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public virtual ICollection<GatewayPlan> GatewayPlans { get; protected set; }

    protected Plan()
    {
    }

    public Plan(Guid id, [NotNull] string name) : base(id)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name), maxLength: PlanConsts.MaxNameLength);

        GatewayPlans = new HashSet<GatewayPlan>();
    }
}
