using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host;

public interface ITenantAppService : ICrudAppService<SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>
{
    Task<SaasTenantDatabasesDto> GetDatabasesAsync();

    Task<SaasTenantConnectionStringsDto> GetConnectionStringsAsync(Guid id);

    Task UpdateConnectionStringsAsync(Guid id, SaasTenantConnectionStringsDto input);

    Task ApplyDatabaseMigrationsAsync(Guid id);

    Task<List<EditionLookupDto>> GetEditionLookupAsync();
    
    Task<bool> CheckConnectionStringAsync(string connectionString);

    Task SetPasswordAsync(Guid id, SaasTenantSetPasswordDto input);
}
