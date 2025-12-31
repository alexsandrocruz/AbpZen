using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplateManagement.TextTemplates;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplateManagement;
using Volo.Abp.Threading;

namespace Volo.Abp.TextTemplateManagement;

[DependsOn(
    typeof(TextTemplateManagementDomainSharedModule),
    typeof(AbpTextTemplatingCoreModule),
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule)
    )]
public class TextTemplateManagementDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private Task _initializeDynamicTextTemplatesTask;
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<TextTemplateManagementOptions>(options =>
            {
                options.SaveStaticTemplatesToDatabase = false;
                options.IsDynamicTemplateStoreEnabled = false;
            });
        }
    }
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        InitializeTextDynamicTemplates(context);
        return Task.CompletedTask;
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    public Task GetInitializeDynamicTextTemplatesTask()
    {
        return _initializeDynamicTextTemplatesTask ?? Task.CompletedTask;
    }

    private void InitializeTextDynamicTemplates(ApplicationInitializationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<TextTemplateManagementOptions>>()
            .Value;

        if (!options.SaveStaticTemplatesToDatabase && !options.IsDynamicTemplateStoreEnabled)
        {
            return;
        }

        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        _initializeDynamicTextTemplatesTask = Task.Run(async () =>
        {
            using var scope = rootServiceProvider.CreateScope();
            var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationTokenProvider = scope.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            var cancellationToken = applicationLifetime?.ApplicationStopping ?? _cancellationTokenSource.Token;

            try
            {
                using (cancellationTokenProvider.Use(cancellationToken))
                {
                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await SaveStaticTemplatesToDatabaseAsync(options, scope, cancellationTokenProvider);

                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await PreCacheDynamicTemplatesAsync(options, scope);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }

    private async static Task SaveStaticTemplatesToDatabaseAsync(
        TextTemplateManagementOptions options,
        IServiceScope scope,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        if (!options.SaveStaticTemplatesToDatabase)
        {
            return;
        }

        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * 10))
            .ExecuteAsync(async _ =>
            {
                try
                {
                    // ReSharper disable once AccessToDisposedClosure
                    await scope
                        .ServiceProvider
                        .GetRequiredService<IStaticTextTemplateSaver>()
                        .SaveAsync();
                }
                catch (Exception ex)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    scope.ServiceProvider
                        .GetService<ILogger<TextTemplateManagementDomainModule>>()?
                        .LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationTokenProvider.Token);
    }

    private async static Task PreCacheDynamicTemplatesAsync(TextTemplateManagementOptions options, IServiceScope scope)
    {
        if (!options.IsDynamicTemplateStoreEnabled)
        {
            return;
        }

        try
        {
            // Pre-cache templates, so first request doesn't wait
            await scope
                .ServiceProvider
                .GetRequiredService<IDynamicTemplateDefinitionStore>()
                .GetAllAsync();
        }
        catch (Exception ex)
        {
            // ReSharper disable once AccessToDisposedClosure
            scope
                .ServiceProvider
                .GetService<ILogger<TextTemplateManagementDomainModule>>()?
                .LogException(ex);

            throw; // It will be cached in InitializeDynamicTemplates
        }
    }
}
