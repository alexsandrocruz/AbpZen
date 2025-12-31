using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Saas.Editions;

[Serializable]
[IgnoreMultiTenancy]
public class EditionDynamicClaimCacheItem
{
    public List<AbpDynamicClaim> Claims { get; set; }

    public EditionDynamicClaimCacheItem()
    {
        Claims = [];
    }

    public EditionDynamicClaimCacheItem(AbpDynamicClaim claims)
    {
        Claims =
        [
            claims
        ];
    }

    public EditionDynamicClaimCacheItem(List<AbpDynamicClaim> claims)
    {
        Claims = claims;
    }

    public static string CalculateCacheKey(Guid tenantId)
    {
        return $"{tenantId}";
    }
}
