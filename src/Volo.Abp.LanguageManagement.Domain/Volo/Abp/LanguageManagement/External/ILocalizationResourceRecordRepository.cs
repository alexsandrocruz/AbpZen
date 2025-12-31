using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.LanguageManagement.External;

public interface ILocalizationResourceRecordRepository : IBasicRepository<LocalizationResourceRecord, Guid>
{
    public LocalizationResourceRecord? Find(
        string name
    );
    
    public Task<LocalizationResourceRecord?> FindAsync(
        string name,
        CancellationToken cancellationToken = default
    );
}