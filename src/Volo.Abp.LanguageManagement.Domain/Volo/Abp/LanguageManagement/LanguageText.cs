using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.LanguageManagement;

public class LanguageText : AuditedEntity<Guid>, IAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual string ResourceName { get; set; }

    public virtual string CultureName { get; set; }

    public virtual string Name { get; set; }

    public virtual string Value { get; set; }

    protected LanguageText()
    {

    }

    public LanguageText(
        Guid id,
        string resourceName,
        string cultureName,
        string name,
        string value = null,
        Guid? tenantId = null)
        : base(id)
    {
        ResourceName = Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName), LanguageTextConsts.MaxResourceNameLength);
        CultureName = Check.NotNullOrWhiteSpace(cultureName, nameof(cultureName), LanguageTextConsts.MaxCultureNameLength);
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), LanguageTextConsts.MaxKeyNameLength);
        Value = Check.Length(value, nameof(value), LanguageTextConsts.MaxValueLength);
        TenantId = tenantId;
    }
}
