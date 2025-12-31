using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.LanguageManagement.External;

public interface ILocalizationTextRecordRepository : IBasicRepository<LocalizationTextRecord, Guid>
{
    List<LocalizationTextRecord> GetList(
        string resourceName,
        string cultureName
    );
    
    Task<List<LocalizationTextRecord>> GetListAsync(
        string resourceName,
        string cultureName,
        CancellationToken cancellationToken = default
    );

    LocalizationTextRecord? Find(
        string resourceName,
        string cultureName
    );
    
    Task<LocalizationTextRecord?> FindAsync(
        string resourceName,
        string cultureName,
        CancellationToken cancellationToken = default
    );
}