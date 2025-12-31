using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace Volo.CmsKit.Admin.Newsletters;

public class NewsletterRecordWithDetailsDto : ExtensibleEntityDto<Guid>, IHasCreationTime
{
    public string EmailAddress { get; set; }

    public ICollection<NewsletterPreferenceDto> Preferences { get; set; }

    public DateTime CreationTime { get; set; }
}
