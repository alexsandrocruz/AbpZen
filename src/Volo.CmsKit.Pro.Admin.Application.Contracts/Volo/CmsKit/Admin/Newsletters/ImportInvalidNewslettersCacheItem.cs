using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Admin.Newsletters;

[Serializable]
[IgnoreMultiTenancy]
public class ImportInvalidNewslettersCacheItem : IDownloadCacheItem
{
    public List<InvalidImportNewslettersFromFileDto> InvalidNewsletters { get; set; }
    
    public string Token { get; set; }
    
    public Guid? TenantId { get; set; }
}