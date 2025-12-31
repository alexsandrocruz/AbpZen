using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;

namespace Volo.Saas.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(SaasDbProperties.ConnectionStringName)]
public interface ISaasDbContext : IEfCoreDbContext
{
    DbSet<Tenant> Tenants { get; }

    DbSet<Edition> Editions { get; }

    DbSet<TenantConnectionString> TenantConnectionStrings { get; }
}
