using System;

namespace Volo.CmsKit.Admin.Newsletters;

public interface IDownloadCacheItem
{
    string Token { get; set; }
    Guid? TenantId { get; set; }
}