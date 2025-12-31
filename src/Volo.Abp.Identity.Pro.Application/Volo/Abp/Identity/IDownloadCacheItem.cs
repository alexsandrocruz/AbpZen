using System;

namespace Volo.Abp.Identity;

public interface IDownloadCacheItem
{
    public string Token { get; set; }

    public Guid? TenantId { get; set; }
}