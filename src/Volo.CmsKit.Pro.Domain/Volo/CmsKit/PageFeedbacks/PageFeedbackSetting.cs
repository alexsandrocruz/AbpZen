using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;

namespace Volo.CmsKit.PageFeedbacks;

public class PageFeedbackSetting : BasicAggregateRoot<Guid>, IMultiTenant
{
    public virtual string EntityType { get; protected set; }
    public virtual string EmailAddresses { get; protected set; }
    public virtual Guid? TenantId { get; protected set; }
    
    protected PageFeedbackSetting()
    {
    }
    
    internal PageFeedbackSetting(Guid id, string entityType, string emailAddresses, Guid? tenantId = null) : base(id)
    {
        SetEntityType(entityType);
        SetEmailAddresses(emailAddresses);
        TenantId = tenantId;
    }
    
    protected virtual PageFeedbackSetting SetEntityType(string entityType)
    {
        EntityType = Check.Length(entityType, nameof(entityType), PageFeedbackConst.MaxEntityTypeLength);
        return this;
    }
    
    public virtual PageFeedbackSetting SetEmailAddresses(string emailAddresses)
    {
        if (string.IsNullOrWhiteSpace(emailAddresses))
        {
            EmailAddresses = string.Empty;
            return this;
        }
        
        Check.Length(emailAddresses, nameof(emailAddresses), PageFeedbackConst.MaxEmailAddressesLength);
        
        foreach (var address in emailAddresses.Split(PageFeedbackConst.EmailAddressesSeparator))
        {
            if (!ValidationHelper.IsValidEmailAddress(address.Trim()))
            {
                throw new ArgumentException($"Email address {address} is not valid");
            }
            
        }
        
        EmailAddresses = emailAddresses;
        return this;
    }
}