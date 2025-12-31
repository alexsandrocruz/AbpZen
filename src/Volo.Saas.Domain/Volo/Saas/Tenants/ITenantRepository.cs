using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace Volo.Saas.Tenants;

public interface ITenantRepository : IBasicRepository<Tenant, Guid>
{
    Task<Tenant> FindByIdAsync(
        Guid id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<Tenant> FindByNameAsync(
        string normalizedName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    [Obsolete("Use FindByIdAsync method.")]
    Tenant FindById(Guid id, bool includeDetails = true);

    [Obsolete("Use FindByNameAsync method.")]
    Tenant FindByName(string normalizedName, bool includeDetails = true);

    Task<List<Tenant>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        Guid? editionId = null,
        DateTime? expirationDateMin = null,
        DateTime? expirationDateMax = null,
        TenantActivationState? tenantActivationState = null,
        DateTime? activationEndDateMin = null,
        DateTime? activationEndDateMax = null,
        CancellationToken cancellationToken = default
    );

    Task<List<Tenant>> GetListWithSeparateConnectionStringAsync(
        string connectionName = ConnectionStrings.DefaultConnectionStringName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter = null,
        Guid? editionId = null,
        DateTime? expirationDateMin = null,
        DateTime? expirationDateMax = null,
        TenantActivationState? tenantActivationState = null,
        DateTime? activationEndDateMin = null,
        DateTime? activationEndDateMax = null,
        CancellationToken cancellationToken = default
    );

    Task UpdateEditionsAsync(
        Guid sourceEditionId,
        Guid? targetEditionId = null,
        CancellationToken cancellationToken = default
    );
}
