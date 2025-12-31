using Volo.Abp.Application;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule),
    typeof(AbpAuditLoggingDomainSharedModule),
    typeof(AbpFeaturesModule))]
public class AbpAuditLoggingApplicationContractsModule : AbpModule
{
}
