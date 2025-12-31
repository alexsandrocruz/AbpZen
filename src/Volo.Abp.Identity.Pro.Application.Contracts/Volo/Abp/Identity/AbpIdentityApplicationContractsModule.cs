using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity;

[DependsOn(
    typeof(AbpIdentityProDomainSharedModule),
    typeof(AbpUsersAbstractionModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpIdentityApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    getApiTypes: new[] { typeof(IdentityUserDto) },
                    createApiTypes: new[] { typeof(IdentityUserCreateDto) },
                    updateApiTypes: new[] { typeof(IdentityUserUpdateDto) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.Role,
                    getApiTypes: new[] { typeof(IdentityRoleDto) },
                    createApiTypes: new[] { typeof(IdentityRoleCreateDto) },
                    updateApiTypes: new[] { typeof(IdentityRoleUpdateDto) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.ClaimType,
                    getApiTypes: new[] { typeof(ClaimTypeDto) },
                    createApiTypes: new[] { typeof(CreateClaimTypeDto) },
                    updateApiTypes: new[] { typeof(UpdateClaimTypeDto) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.OrganizationUnit,
                    getApiTypes: new[] { typeof(OrganizationUnitDto), typeof(OrganizationUnitWithDetailsDto) },
                    createApiTypes: new[] { typeof(OrganizationUnitCreateDto) },
                    updateApiTypes: new[] { typeof(OrganizationUnitUpdateDto) }
                );
        });
    }
}
