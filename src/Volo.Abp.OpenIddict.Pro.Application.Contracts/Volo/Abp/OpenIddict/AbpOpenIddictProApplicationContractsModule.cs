using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.OpenIddict.Applications.Dtos;
using Volo.Abp.OpenIddict.Scopes.Dtos;
using Volo.Abp.Threading;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpOpenIddictProDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpOpenIddictProApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    OpenIddictModuleExtensionConsts.ModuleName,
                    OpenIddictModuleExtensionConsts.EntityNames.Application,
                    getApiTypes: new[] { typeof(ApplicationDto) },
                    createApiTypes: new[] { typeof(CreateApplicationInput) },
                    updateApiTypes: new[] { typeof(UpdateApplicationInput) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    OpenIddictModuleExtensionConsts.ModuleName,
                    OpenIddictModuleExtensionConsts.EntityNames.Scope,
                    getApiTypes: new[] { typeof(ScopeDto) },
                    createApiTypes: new[] { typeof(CreateScopeInput) },
                    updateApiTypes: new[] { typeof(UpdateScopeInput) }
                );
        });
    }
}
