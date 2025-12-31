using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.UrlShorting;

public class ShortenedUrl : BasicAggregateRoot<Guid>, IMultiTenant, ICreationAuditedObject
{
    [NotNull]
    public virtual string Source { get; protected set; }

    [NotNull]
    public virtual string Target { get; protected set; }
    
    public virtual bool IsRegex { get; protected set; }

    public virtual Guid? TenantId { get; protected set; }

    public DateTime CreationTime { get; protected set; }

    public Guid? CreatorId { get; protected set; }

    protected ShortenedUrl()
    {

    }

    public ShortenedUrl(
        Guid id,
        [NotNull] string source,
        [NotNull] string target,
        bool isRegex = false,
        [CanBeNull] Guid? tenantId = null
    ) : base(id)
    {
        Source = Check.NotNullOrWhiteSpace(source, nameof(source), ShortenedUrlConst.MaxSourceLength);
        SetTarget(target);
        TenantId = tenantId;
        IsRegex = isRegex;
    }

    public virtual void SetTarget(string target)
    {
        Target = Check.NotNullOrWhiteSpace(target, nameof(target), ShortenedUrlConst.MaxTargetLength);
    }
}
