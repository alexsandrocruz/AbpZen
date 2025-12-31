using System;

namespace Volo.Saas.Editions;

public class EditionDynamicClaimsPrincipalContributorCacheOptions
{
    public TimeSpan CacheAbsoluteExpiration { get; set; }

    public EditionDynamicClaimsPrincipalContributorCacheOptions()
    {
        CacheAbsoluteExpiration = TimeSpan.FromHours(1);
    }
}
