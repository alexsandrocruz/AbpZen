using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.LanguageManagement;

public interface ILanguageRepository : IBasicRepository<Language, Guid>
{
    Task<List<Language>> GetListByIsEnabledAsync(
        bool isEnabled,
        CancellationToken cancellationToken = default);

    Task<List<Language>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        string cultureName,
        CancellationToken cancellationToken = default);
}
