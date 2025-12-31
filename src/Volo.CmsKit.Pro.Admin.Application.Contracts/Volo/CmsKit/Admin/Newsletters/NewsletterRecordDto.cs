using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace Volo.CmsKit.Admin.Newsletters;

public class NewsletterRecordDto : ExtensibleEntityDto<Guid>, IHasCreationTime
{
    public string EmailAddress { get; set; }

    public DateTime CreationTime { get; set; }
    
    public List<string> Preferences { get; set; }
}
