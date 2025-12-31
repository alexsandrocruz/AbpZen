using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;

namespace Volo.CmsKit.Newsletters;

public class NewsletterSummaryQueryResultItem : IHasCreationTime
{
    public Guid Id { get; set; }

    public string EmailAddress { get; set; }

    public DateTime CreationTime { get; set; }
    
    public List<string> Preferences { get; set; }
}
