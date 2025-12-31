using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Polls;
public class PollOption : Entity<Guid>, IMultiTenant
{
    public virtual Guid PollId { get; protected set; }
    public virtual string Text { get; protected set; }
    public virtual int VoteCount { get; protected set; }
    public virtual Guid? TenantId { get; protected set; }
    public virtual int Order { get; protected set; }


    protected PollOption()
    {

    }

    internal PollOption(
        Guid id,
        string text,
        int order,
        Guid pollId,
        Guid? tenantId
        ) : base(id)
    {
        SetText(text);
        SetOrder(order);
        PollId = pollId;
        Order = order;
        TenantId = tenantId;
    }

    public virtual void SetText(string text)
    {
        Text = Check.NotNullOrEmpty(text, nameof(text), PollConst.MaxTextLength);
    }

    public virtual void SetOrder(int order)
    {
        Order = order;
    }

    public virtual void Increase()
    {
        VoteCount++;
    }
}
