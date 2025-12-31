using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;

namespace Volo.Abp.AuditLogging;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpAuditLoggingApplicationContractsModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpSettingManagementDomainModule))]
public class AbpAuditLoggingApplicationModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpAuditLoggingApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpAuditLoggingApplicationAutoMapperProfile>(validate: true);
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    AuditLoggingModuleExtensionConsts.ModuleName,
                    AuditLoggingModuleExtensionConsts.EntityNames.AuditLog,
                    getApiTypes: new[] { typeof(AuditLogDto) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    AuditLoggingModuleExtensionConsts.ModuleName,
                    AuditLoggingModuleExtensionConsts.EntityNames.AuditLogAction,
                    getApiTypes: new[] { typeof(AuditLogAction) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    AuditLoggingModuleExtensionConsts.ModuleName,
                    AuditLoggingModuleExtensionConsts.EntityNames.EntityChange,
                    getApiTypes: new[] { typeof(EntityChangeDto) }
                );
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        

        await context.ServiceProvider
            .GetRequiredService<IBackgroundWorkerManager>()
            .AddAsync(context.ServiceProvider.GetRequiredService<ExpiredAuditLogDeleterWorker>());
    }
}
