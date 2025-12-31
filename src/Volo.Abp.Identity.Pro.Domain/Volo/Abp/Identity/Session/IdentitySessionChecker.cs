using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Timing;

namespace Volo.Abp.Identity.Session;

public class IdentitySessionChecker : ITransientDependency
{
    public ILogger<IdentitySessionChecker> Logger { get; set; }

    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; }
    protected IdentitySessionManager IdentitySessionManager { get; }
    protected IDistributedCache<IdentitySessionCacheItem> Cache { get; }
    protected IClock Clock { get; }
    protected IWebClientInfoProvider WebClientInfoProvider { get; }
    protected IOptions<IdentitySessionCheckerOptions> Options { get; }

    public IdentitySessionChecker(
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOption,
        IdentitySessionManager identitySessionManager,
        IDistributedCache<IdentitySessionCacheItem> cache,
        IClock clock,
        IWebClientInfoProvider webClientInfoProvider,
        IOptions<IdentitySessionCheckerOptions> options)
    {
        Logger = NullLogger<IdentitySessionChecker>.Instance;

        AbpClaimsPrincipalFactoryOptions = abpClaimsPrincipalFactoryOption;
        IdentitySessionManager = identitySessionManager;
        Cache = cache;
        Clock = clock;
        WebClientInfoProvider = webClientInfoProvider;
        Options = options;
    }

    public virtual async Task<bool> IsValidateAsync(string sessionId)
    {
        if (!AbpClaimsPrincipalFactoryOptions.Value.IsDynamicClaimsEnabled)
        {
            Logger.LogDebug($"Dynamic claims is disabled, The SessionId({sessionId}) will not be checked.");
            return true;
        }

        if (sessionId.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("SessionId is null or empty cannot be checked.");
            return false;
        }

        var sessionCacheItem = await Cache.GetOrAddAsync(sessionId, async () =>
        {
            Logger.LogDebug($"Get SessionId({sessionId}) from IdentitySessionManager.");

            var session = await IdentitySessionManager.FindAsync(sessionId, false);
            if (session == null)
            {
                Logger.LogWarning($"Could not find SessionId({sessionId}) in the database.");
                return null;
            }

            Logger.LogDebug($"Found SessionId({sessionId}) in the database.");
            return new IdentitySessionCacheItem
            {
                Id = session.Id,
                SessionId = session.SessionId
            };
        });

        if (sessionCacheItem == null)
        {
            await Cache.RemoveAsync(sessionId);
            return false;
        }

        sessionCacheItem.CacheLastAccessed = Clock.Now;
        sessionCacheItem.IpAddress = WebClientInfoProvider.ClientIpAddress;
        sessionCacheItem.HitCount++;

        Logger.LogDebug($"SessionId({sessionId}) found in cache, " +
                        $"Updating hit count({sessionCacheItem.HitCount}), " +
                        $"last access time({sessionCacheItem.CacheLastAccessed}) and " +
                        $"IP address({sessionCacheItem.IpAddress}).");

        if (sessionCacheItem.HitCount == 1)
        {
            Logger.LogDebug($"Updating the session from cache on the first check.");
            await IdentitySessionManager.UpdateSessionFromCacheAsync(sessionId, sessionCacheItem);
        }
        else if (sessionCacheItem.HitCount > Options.Value.UpdateSessionAfterCacheHit)
        {
            Logger.LogDebug($"Update the session from cache because reached the maximum cache hit count({Options.Value.UpdateSessionAfterCacheHit}).");
            sessionCacheItem.HitCount = 0;
            await IdentitySessionManager.UpdateSessionFromCacheAsync(sessionId, sessionCacheItem);
        }

        await Cache.SetAsync(sessionId, sessionCacheItem);

        return true;
    }
}
