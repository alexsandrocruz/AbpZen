using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Newsletters;

public class NewsletterPreference : CreationAuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid NewsletterRecordId { get; protected set; }

    public virtual string Preference { get; protected set; }

    public virtual string Source { get; protected set; }

    public virtual string SourceUrl { get; protected set; }

    public virtual Guid? TenantId { get; protected set; }

    protected NewsletterPreference()
    {

    }

    public NewsletterPreference(
        Guid id,
        Guid newsletterRecordId,
        [NotNull] string preference,
        [NotNull] string source,
        [NotNull] string sourceUrl,
        Guid? tenantId = null)
        : base(id)
    {
        NewsletterRecordId = newsletterRecordId;
        Preference = Check.NotNullOrWhiteSpace(preference, nameof(preference), NewsletterPreferenceConst.MaxPreferenceLength);
        Source = Check.NotNullOrWhiteSpace(source, nameof(source), NewsletterPreferenceConst.MaxSourceLength);
        SourceUrl = Check.NotNullOrWhiteSpace(sourceUrl, nameof(sourceUrl), NewsletterPreferenceConst.MaxSourceUrlLength);
        TenantId = tenantId;
    }
}
