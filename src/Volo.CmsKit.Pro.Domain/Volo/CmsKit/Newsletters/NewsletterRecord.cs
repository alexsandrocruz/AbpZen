using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Newsletters;

public class NewsletterRecord : CreationAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual string EmailAddress { get; protected set; }

    public virtual ICollection<NewsletterPreference> Preferences { get; protected set; }

    public virtual Guid? TenantId { get; protected set; }

    protected NewsletterRecord()
    {

    }

    public NewsletterRecord(
        Guid id,
        [NotNull] string emailAddress,
        Guid? tenantId = null)
        : base(id)
    {
        SetEmailAddress(emailAddress);
        Preferences = new Collection<NewsletterPreference>();
        TenantId = tenantId;
    }

    public NewsletterRecord SetEmailAddress([NotNull] string emailAddress)
    {
        EmailAddress = Check.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress), NewsletterRecordConst.MaxEmailAddressLength);

        return this;
    }

    public NewsletterRecord AddPreferences(NewsletterPreference preference)
    {
        Preferences.AddIfNotContains(preference);

        return this;
    }

    public NewsletterRecord RemovePreference(Guid id)
    {
        var newsletterPreference = Preferences.First(x => x.Id == id);
        Preferences.Remove(newsletterPreference);

        return this;
    }
}
