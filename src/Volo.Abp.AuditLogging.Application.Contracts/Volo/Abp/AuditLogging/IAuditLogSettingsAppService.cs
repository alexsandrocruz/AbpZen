using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AuditLogging;

public interface IAuditLogSettingsAppService : IApplicationService
{
    Task<AuditLogSettingsDto> GetAsync();

    Task UpdateAsync(AuditLogSettingsDto input);

    Task<AuditLogGlobalSettingsDto> GetGlobalAsync();

    Task UpdateGlobalAsync(AuditLogGlobalSettingsDto input);
}