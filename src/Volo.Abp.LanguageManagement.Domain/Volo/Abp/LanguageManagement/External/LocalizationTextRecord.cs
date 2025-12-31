using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.LanguageManagement.External;

public class LocalizationTextRecord : BasicAggregateRoot<Guid>, IHasCreationTime, IHasModificationTime
{
    public virtual string ResourceName { get; private set; }

    public virtual string CultureName { get; private set; }

    public virtual string Value { get; set; }
    
    public DateTime CreationTime { get; protected set; }
    
    public DateTime? LastModificationTime { get; set; }

    protected LocalizationTextRecord()
    {
        
    }
    
    public LocalizationTextRecord(
        Guid id,
        string resourceName,
        string cultureName,
        string value)
        : base(id)
    {
        ResourceName = Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName), LocalizationTextRecordConsts.MaxResourceNameLength);
        CultureName = Check.NotNullOrWhiteSpace(cultureName, nameof(cultureName), LocalizationTextRecordConsts.MaxCultureNameLength);
        Value = Check.Length(value, nameof(value), LocalizationTextRecordConsts.MaxValueLength);
    }
}