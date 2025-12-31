using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.LanguageManagement;

public interface ILanguageTextRepository : IBasicRepository<LanguageText, Guid>
{
    List<LanguageText> GetList(
        string resourceName,
        string cultureName
    );
    
    Task<List<LanguageText>> GetListAsync(
        string resourceName,
        string cultureName,
        CancellationToken cancellationToken = default
    );
}
