using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Faqs;

public class FaqSection : FullAuditedAggregateRoot<Guid>
{
    public virtual string GroupName { get; protected set; }
    
    public virtual string Name { get; protected set; }
    
    public virtual int Order { get; set; }
    
    protected FaqSection()
    {
    
    }

    public FaqSection(
        Guid id,
        [NotNull] string groupName,
        [NotNull] string name,
        int order = 0) : base(id)
    {
        SetGroupName(groupName);
        SetName(name);
        Order = order;
    }
    
    public virtual FaqSection SetGroupName([NotNull] string groupName)
    {
        GroupName = Check.NotNullOrWhiteSpace(groupName, nameof(groupName), FaqSectionConst.MaxGroupNameLength);
        return this;
    }
    
    public virtual FaqSection SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), FaqSectionConst.MaxNameLength);
        return this;
    }
}