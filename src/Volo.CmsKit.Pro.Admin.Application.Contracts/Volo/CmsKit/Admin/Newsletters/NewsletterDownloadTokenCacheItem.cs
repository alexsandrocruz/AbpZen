using System;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Admin.Newsletters;

[Serializable]
[IgnoreMultiTenancy]
public class NewsletterDownloadTokenCacheItem : IDownloadCacheItem
{
    public string Token { get; set; }

    public Guid? TenantId { get; set; }
}