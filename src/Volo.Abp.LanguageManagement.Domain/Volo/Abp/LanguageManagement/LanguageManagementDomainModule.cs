using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.LanguageManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.LanguageManagement;

[DependsOn(
    typeof(LanguageManagementDomainSharedModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule)
)]
public class LanguageManagementDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                LanguageManagementModuleExtensionConsts.ModuleName,
                LanguageManagementModuleExtensionConsts.EntityNames.Language,
                typeof(Language)
            );
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.GlobalContributors.Add<DynamicLocalizationResourceContributor>();
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Volo.Abp.LanguageManagement", typeof(LanguageManagementResource));
        });

        context.Services.AddAutoMapperObjectMapper<LanguageManagementDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<LanguageManagementDomainAutoMapperProfile>(validate: true);
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<Language, LanguageEto>(typeof(LanguageManagementDomainModule));
            options.EtoMappings.Add<LanguageText, LanguageTextEto>(typeof(LanguageManagementDomainModule));
        });

        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<AbpExternalLocalizationOptions>(options =>
            {
                options.SaveToExternalStore = false;
            });
        }
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        
        await SaveLocalizationsAsync(context);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private async Task SaveLocalizationsAsync(ApplicationInitializationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<AbpExternalLocalizationOptions>>()
            .Value;

        if (!options.SaveToExternalStore)
        {
            return;
        }

        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        Task.Run(async () =>
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

                    await Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(
                            8,
                            retryAttempt => TimeSpan.FromSeconds(
                                RandomHelper.GetRandom(
                                    (int)Math.Pow(2, retryAttempt) * 8,
                                    (int)Math.Pow(2, retryAttempt) * 12)
                            )
                        )
                        .ExecuteAsync(async _ =>
                        {
                            try
                            {
                                // ReSharper disable once AccessToDisposedClosure
                                await scope
                                    .ServiceProvider
                                    .GetRequiredService<IExternalLocalizationSaver>()
                                    .SaveAsync();
                            }
                            catch (Exception ex)
                            {
                                // ReSharper disable once AccessToDisposedClosure
                                scope.ServiceProvider
                                    .GetService<ILogger<AbpLocalizationModule>>()?
                                    .LogException(ex);

                                throw; // Polly will catch it
                            }
                        }, cancellationTokenProvider.Token);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }
}