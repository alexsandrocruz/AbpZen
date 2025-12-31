using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Saas.Editions;

public class Edition : FullAuditedAggregateRoot<Guid>, IHasEntityVersion
{
    [NotNull]
    public virtual string DisplayName { get; protected set; }

    [CanBeNull]
    public virtual Guid? PlanId { get; set; }

    public virtual int EntityVersion { get; protected set; }


    protected Edition()
    {

    }

    public Edition(Guid id, [NotNull] string displayName) : base(id)
    {
        SetDisplayName(displayName);
    }

    public virtual void SetDisplayName([NotNull] string displayName)
    {
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), EditionConsts.MaxDisplayNameLength);
    }
}
