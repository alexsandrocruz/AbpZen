using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity.Session;

public class IdentitySessionCleanupService : ITransientDependency
{
    public ILogger<IdentitySessionCleanupService> Logger { get; set; }
    protected IdentitySessionCleanupOptions CleanupOptions { get; }
    protected IdentitySessionManager IdentitySessionManager { get; }

    public IdentitySessionCleanupService(
        IOptionsMonitor<IdentitySessionCleanupOptions> cleanupOptions,
        IdentitySessionManager identitySessionManager)
    {
        Logger = NullLogger<IdentitySessionCleanupService>.Instance;;

        CleanupOptions = cleanupOptions.CurrentValue;
        IdentitySessionManager = identitySessionManager;
    }

    public virtual async Task CleanAsync()
    {
        Logger.LogInformation("Start cleanup sessions.");
        try
        {
            await IdentitySessionManager.DeleteAllAsync(CleanupOptions.InactiveTimeSpan);
        }
        catch (Exception exception)
        {
            Logger.LogException(exception);
        }
        finally
        {
            Logger.LogInformation("Cleanup sessions completed.");
        }
    }
}
