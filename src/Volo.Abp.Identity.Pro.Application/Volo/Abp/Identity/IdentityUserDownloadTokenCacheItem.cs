using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

[Serializable]
[IgnoreMultiTenancy]
public class IdentityUserDownloadTokenCacheItem : IDownloadCacheItem
{
    public string Token { get; set; }
    
    public Guid? TenantId { get; set; }
}