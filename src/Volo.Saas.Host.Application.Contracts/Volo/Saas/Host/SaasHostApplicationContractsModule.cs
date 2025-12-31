using Volo.Abp;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Saas.Host.Dtos;
using Volo.Abp.Threading;
using Volo.Payment;

namespace Volo.Saas.Host;

[DependsOn(
    typeof(SaasDomainSharedModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpPaymentApplicationContractsModule)
    )]
public class SaasHostApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    SaasModuleExtensionConsts.ModuleName,
                    SaasModuleExtensionConsts.EntityNames.Tenant,
                    getApiTypes: new[] { typeof(SaasTenantDto) },
                    createApiTypes: new[] { typeof(SaasTenantCreateDto) },
                    updateApiTypes: new[] { typeof(SaasTenantUpdateDto) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    SaasModuleExtensionConsts.ModuleName,
                    SaasModuleExtensionConsts.EntityNames.Edition,
                    getApiTypes: new[] { typeof(EditionDto) },
                    createApiTypes: new[] { typeof(EditionCreateDto) },
                    updateApiTypes: new[] { typeof(EditionUpdateDto) }
                );
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
