using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.AuditLogging;

public class ExpiredAuditLogDeleterWorker : AsyncPeriodicBackgroundWorkerBase
{
    public ExpiredAuditLogDeleterWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<ExpiredAuditLogDeleterOptions> options)
        : base(timer, serviceScopeFactory)
    {
        timer.Period = options.Value.Period;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        if (!(await workerContext
                .ServiceProvider
                .GetRequiredService<ISettingProvider>()
                .IsTrueAsync(AuditLogSettingNames.IsPeriodicDeleterEnabled)))
        {
            return;
        }

        Logger.LogInformation("Expired audit log deleter worker started");

        await workerContext
            .ServiceProvider
            .GetRequiredService<ExpiredAuditLogDeleterService>()
            .DeleteAsync();

        Logger.LogInformation("Expired audit log deleter worker finished");
    }
}