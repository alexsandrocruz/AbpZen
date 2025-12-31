using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin;

[DependsOn(
    typeof(AbpPaymentApplicationContractsModule),
    typeof(AbpAuthorizationModule))]
public class AbpPaymentAdminApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentAdminApplicationContractsModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
            PaymentModuleExtensionConsts.ModuleName,
            PaymentModuleExtensionConsts.EntityNames.Plan,
            getApiTypes: new[] { typeof(PlanDto) },
            createApiTypes: new[] { typeof(PlanCreateInput) },
            updateApiTypes: new[] { typeof(PlanUpdateInput) }
        );

        ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
            PaymentModuleExtensionConsts.ModuleName,
            PaymentModuleExtensionConsts.EntityNames.GatewayPlan,
            getApiTypes: new[] { typeof(GatewayPlanDto) },
            createApiTypes: new[] { typeof(GatewayPlanCreateInput) },
            updateApiTypes: new[] { typeof(GatewayPlanUpdateInput) }
        );
    }
}
